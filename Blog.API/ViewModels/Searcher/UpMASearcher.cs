﻿using Blog.API.ViewModels.Fillers;
using Blog.Data;
using Blog.Model;
using Microsoft.AspNetCore.Http;
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
    /// 突破MA均线
    /// </summary>
    public class UpMASearcher : BaseDoWorkViewModel
    {



        private readonly ILogger _logger;

        private ArgUpMA _arg;

        public UpMASearcher(IServiceScopeFactory serviceScopeFactory,
                string userId, IConfiguration configuration,
            ILogger logger, ArgUpMA arg) : base(serviceScopeFactory, userId, configuration)
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

                int numcnt = _arg.UpDaysNum + _arg.NearDaysNum + _arg.AvgDays;

                //当天的数据算一个
                var dayDataList = await helper.GetDayData(stockId, numcnt, _arg.BaseDate);

                //使用成交量，需要复权
                //复权，会改变dayDataList中的数据
                await Utils.CalcData.FuQuan(db, stockId, dayDataList);

                bool fitFlag = false;

                if (dayDataList.Count == numcnt)
                {
                    //
                    //获取均线数据
                    var closeList = (from ii in dayDataList
                                     select ii.Close).ToList();

                    var maArray = Utils.CalcData.Average(closeList, _arg.AvgDays);

                    //满足条件的flag
                    //对数据进行处理,当日的不处理
                    for (int i = 0; i < _arg.UpDaysNum; i++)
                    {
                        var current = dayDataList[i];

                        if (current.Close > maArray[i])
                        {

                            //不满足条件
                            fitFlag = true;
                            dayData = current;
                            break;
                        }
                    }
                    if (fitFlag)
                    {
                        int exceptCnt = 0;
                        for (int j = 0; j < _arg.NearDaysNum; j++)
                        {

                            //进一步判读之前的是否是在均线之下
                            var temp = dayDataList[j + _arg.UpDaysNum];
                            if (temp.Close >= maArray[j + _arg.UpDaysNum])
                            {
                                //不满足条件
                                exceptCnt++;
                                if (exceptCnt > _arg.ExceptionNum)
                                {
                                    fitFlag = false;
                                    break;
                                }
                            }
                        }


                    }
                }



                //如果是null，表示不符合筛选条件

                RealTimeData real = null;
                if (fitFlag)
                {
                    real = await helper.ConvertDayToReal(dayData);
                }

                return real;
            }
        }

        private void prepareSearch()
        {
            _taskStartTime = DateTime.Now;

            _actionName = "upMA";
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
            //必须至少间隔10
            if (_arg.NearDaysNum < 1 || _arg.AvgDays < 1 || _arg.AvgDays > 240)
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
