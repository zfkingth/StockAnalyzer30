﻿using AutoMapper;
using Blog.API.ViewModels.Fillers;
using Blog.Data;
using Blog.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.API.ViewModels.Searcher
{
    /// <summary>
    /// 平台突破
    /// </summary>
    public class CloseBreakSearcher : BaseDoWorkViewModel
    {



        private readonly ILogger _logger;

        private ArgCloseBreak _arg;

        public CloseBreakSearcher(IServiceScopeFactory serviceScopeFactory,
                string userId, IConfiguration configuration,
            ILogger logger, ArgCloseBreak arg) : base(serviceScopeFactory, userId, configuration)
        {
            _logger = logger;
            _arg = arg;

        }










        private async Task<RealTimeData> Filter(string stockId)
        {
            DayData dayData = null;


            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<BlogContext>();

                var helper = new Utils.Utility(db);

                //当天的数据算一个
                var dayDataList = await helper.GetDayData(stockId, _arg.CircleDaysNum + _arg.NearDaysNum, _arg.BaseDate);

                //使用成交量，需要复权
                //复权，会改变dayDataList中的数据
                await Utils.CalcData.FuQuan(db, stockId, dayDataList);



                if (dayDataList.Count == _arg.CircleDaysNum + _arg.NearDaysNum)
                {


                    //求出最近的最高价格,收盘
                    int breakIndex = 0;
                    int i = 0;
                    float maxClosePrice = 0;
                    for (; i < _arg.NearDaysNum; i++)
                    {
                        float current = dayDataList[i].Close;
                        if (current > maxClosePrice)
                        {
                            maxClosePrice = current;
                            breakIndex = i;
                        }
                    }

                    //求出之前的最高的盘中最高价格
                    float previousMaxHigh = 0;

                    for (; i < dayDataList.Count - _arg.NearDaysNum; i++)
                    {
                        float current = dayDataList[i].High;
                        if (current > previousMaxHigh)
                        {
                            previousMaxHigh = current;
                        }
                    }

                    //符合回调条件的点
                    bool match = false;
                    if (maxClosePrice > previousMaxHigh)
                    {
                        for (i = 0; i < _arg.CircleDaysNum; i++)
                        {
                            //从最早的数据开始遍历
                            var listItem = dayDataList[dayDataList.Count - 1 - i];
                            if (listItem.High >= previousMaxHigh)
                            {
                                //寻找到高点

                                //后面的最低价格
                                float suffixMinLow = float.MaxValue;

                                //寻找后续最低点
                                for (int j = i + 1; j < _arg.CircleDaysNum; j++)
                                {
                                    float current = dayDataList[dayDataList.Count - 1 - j].Low;
                                    if (current < suffixMinLow)
                                    {
                                        suffixMinLow = current;
                                    }
                                }

                                float fudu = (listItem.High - suffixMinLow) / listItem.High;

                                if (fudu >= _arg.HuiTiaoFuDuLow / 100f && fudu <= _arg.HuiTiaoFuDuHigh / 100f)
                                {
                                    //符合回调幅度的高点
                                    match = true;
                                    break;
                                }


                            }

                        }

                    }
                    if (match)
                    {

                        //符合条件
                        dayData = dayDataList[0];

                    }
                }



                //如果是null，表示不符合筛选条件

                RealTimeData real = null;
                if (dayData != null)
                {
                    real = await helper.ConvertDayToReal(dayData);
                }

                return real;
            }
        }




        private void prepareSearch()
        {
            _taskStartTime = DateTime.Now;

            _actionName = "closeBreak";
            if (_arg.SearchFromAllStocks)
            {
                _arg.StockIdList = new List<string>();
                //只缓存从所有股票作为参数的任务结果
                _needStoreInCache = true;
                _argString = JsonConvert.SerializeObject(_arg);
            }

        }



        /// <summary>
        /// 价格突破
        /// </summary>
        /// <param name="StockList"></param>
        /// <param name="pm"></param>
        /// <returns></returns>
        public override async Task<List<RealTimeData>> Search()
        {
            if (_arg.NearDaysNum > _arg.CircleDaysNum)
            {
                throw new Exception("参数不正确");
            }
            prepareSearch();

            List<RealTimeData> list = null;

            if (_arg.SearchFromAllStocks)
                _arg.StockIdList = GetAllStockIdWithOutIndex();

            list = await DoSearch(_arg.StockIdList, Filter);

            return list;

        }

    }

}
