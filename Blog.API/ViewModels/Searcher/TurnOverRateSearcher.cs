using Blog.API.ViewModels.Fillers;
using Blog.Data;
using Blog.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.API.ViewModels.Searcher
{
    /// <summary>
    /// 根据换手率
    /// </summary>
    public class TurnOverRateSearcher : BaseDoWorkViewModel
    {



        private readonly ILogger _logger;

        private ArgTurnOverRate _arg;

        public TurnOverRateSearcher(IServiceScopeFactory serviceScopeFactory,
                string userId, IConfiguration configuration,
            ILogger logger, ArgTurnOverRate arg) : base(serviceScopeFactory, userId, configuration)
        {
            _logger = logger;
            _arg = arg;

        }










        private async Task<RealTimeData> Filter(string stockId)
        {


            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<BlogContext>();

                var helper = new Utils.Utility(db);

                float low = _arg.TurnOverRateLow;
                float high = _arg.TurnOverRateHigh;
                RealTimeData ret = null;


                var realData = await helper.GetRealTimeDataWithDate(stockId, _arg.BaseDate);
                if (realData != null)
                {
                    var zhibiao = realData.HuanShouLiu;

                    if (zhibiao != null)
                    {
                        if (low <= zhibiao && zhibiao <= high)
                        {
                            ret = realData;
                        }

                    }
                }



                //如果是null，表示不符合筛选条件

                return ret; ;

            }
        }



    
        public override async Task<List<RealTimeData>> Search()
        {

            List<RealTimeData> list = null;

            if (_arg.SearchFromAllStocks)
                _arg.StockIdList = GetAllStockIdWithOutIndex();

            list = await DoSearch(_arg.StockIdList, Filter);

            return list;

        }

    }

}
