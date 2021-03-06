﻿using Blog.API.Utils;
using Blog.Data;
using Blog.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.API.ViewModels.Fillers
{
    public class PullAllStockNamesViewModel : BaseDoWorkViewModel
    {


        private readonly ILogger _logger;
        public PullAllStockNamesViewModel(IServiceScopeFactory serviceScopeFactory,

            ILogger<PullAllStockNamesViewModel> logger) : base(serviceScopeFactory)
        {
            _logger = logger;
        }




        public async Task pullShanghai()
        {


            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip
                                         | DecompressionMethods.Deflate
            };
            using (var client = new HttpClient(handler))
            {
                Stream val = null;
                client.BaseAddress = new Uri("http://query.sse.com.cn/security/stock/downloadStockListFile.do?csrcCode=&stockCode=&areaName=&stockType=1");


                client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", " Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "http://www.sse.com.cn/assortment/stock/list/share/");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "zh-CN,zh;q=0.8");



                HttpResponseMessage response = await client.GetAsync("");

                if (response.IsSuccessStatusCode)
                {
                    val = await response.Content.ReadAsStreamAsync();


                }
                else
                {
                    throw new Exception($"从上海证券交易所获取股票代码 通讯错误");
                }



                ReturnInfo ro = deserializeShanghai(val);

                WriteToNamesDbParalle(ro);



            }

        }

        /// <summary>
        /// 获取科创板代码
        /// </summary>
        /// <returns></returns>
        public async Task pullShanghaiKCB()
        {


            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip
                                         | DecompressionMethods.Deflate
            };
            using (var client = new HttpClient(handler))
            {
                Stream val = null;
                client.BaseAddress = new Uri("http://query.sse.com.cn/security/stock/downloadStockListFile.do?csrcCode=&stockCode=&areaName=&stockType=8");


                client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", " Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "http://www.sse.com.cn/assortment/stock/list/share/");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "zh-CN,zh;q=0.8");



                HttpResponseMessage response = await client.GetAsync("");

                if (response.IsSuccessStatusCode)
                {
                    val = await response.Content.ReadAsStreamAsync();


                }
                else
                {
                    throw new Exception($"从上海证券交易所获取股票代码 通讯错误");
                }



                ReturnInfo ro = deserializeShanghai(val);

                WriteToNamesDbParalle(ro);



            }

        }

        public async Task pullShenzhen()
        {
            //从深圳获取股票
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip
                                   | DecompressionMethods.Deflate
            };

            using (var client = new HttpClient(handler))
            {
                Stream val = null;
                client.BaseAddress = new Uri("http://www.szse.cn/api/report/ShowReport?SHOWTYPE=xlsx&CATALOGID=1110&TABKEY=tab1&random=0.19832070121584655");


                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 10.0; WOW64; Trident/7.0; .NET4.0C; .NET4.0E; .NET CLR 2.0.50727; .NET CLR 3.0.30729; .NET CLR 3.5.30729)");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "http://www.szse.cn/market/stock/list/");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "zh-CN,zh;q=0.8");



                HttpResponseMessage response = await client.GetAsync("");

                if (response.IsSuccessStatusCode)
                {
                    val = await response.Content.ReadAsStreamAsync();


                }
                else
                {
                    throw new Exception($"从深圳证券交易所获取股票代码 通讯错误");
                }




                ReturnInfo ro = deserializeShenzhen(val);

                WriteToNamesDbParalle(ro);









            }
        }
        public async Task PullAll()
        {
            await setStartDate(SystemEvents.PullStockNames);

            await pullShenzhen();
            await pullShanghai();

            await pullShanghaiKCB();

            await setFinishedDate(SystemEvents.PullStockNames);

        }

            private ReturnInfo deserializeShenzhen(Stream stream)
        {
            ReturnInfo ro = new ReturnInfo();







            try
            {   // Open the text file using a stream reader.
                using (var pck = new OfficeOpenXml.ExcelPackage())
                {

                    pck.Load(stream);

                    var ws = pck.Workbook.Worksheets.First();

                    //跳过第一行
                    for (int rowNum = ws.Dimension.Start.Row + 1; rowNum <= ws.Dimension.End.Row; rowNum++)
                    {


                        string stockSymbol = ws.Cells[rowNum, 1].Text;
                        string stockName = ws.Cells[rowNum, 2].Text;
                        if (string.IsNullOrWhiteSpace(stockSymbol)) break;
                        var si = new StockInfo();
                        si.symbol = stockSymbol.Trim();
                        si.name = stockName.Trim();//一个是中文空格，一个是英文空格
                        switch (si.symbol.Substring(0, 2))
                        {
                            case "60":
                            case "30":
                            case "00":

                                ro.items.Add(si);
                                break;
                            default: break;

                        }

                    }

                }


            }
            catch (IOException e)
            {
                _logger.LogError(e.Message);
            }


            //       string stockName_Symbol = aItem.InnerText.Trim();



            //var slist = stockName_Symbol.Split('(', ')');
            //var stockInfo = new StockInfo();
            //stockInfo.name = slist[0];
            //stockInfo.symbol = slist[1];
            //switch (stockInfo.symbol.Substring(0, 2))
            //{
            //    case "60":
            //    case "30":
            //    case "00":

            //        ro.items.Add(stockInfo);
            //        break;
            //    default: break;

            //}




            return ro;

        }




        private ReturnInfo deserializeShanghai(Stream stream)
        {
            ReturnInfo ro = new ReturnInfo();







            try
            {   // Open the text file using a stream reader.
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                var encoding = Encoding.GetEncoding("GB2312");

                using (StreamReader sr = new StreamReader(stream, encoding))
                {
                    // Read the stream to a string, and write the string to the console.
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine().Trim();
                        var words = line.Split('\t');
                        var si = new StockInfo();
                        si.symbol = words[0].Trim();
                        si.name = words[1].Trim();
                        switch (si.symbol.Substring(0, 2))
                        {
                            case "60":
                            case "30":
                            case "00":
                            case "68":

                                ro.items.Add(si);
                                break;
                            default: break;

                        }
                    }

                }
            }
            catch (IOException e)
            {
                _logger.LogError(e.Message);
            }


            //       string stockName_Symbol = aItem.InnerText.Trim();



            //var slist = stockName_Symbol.Split('(', ')');
            //var stockInfo = new StockInfo();
            //stockInfo.name = slist[0];
            //stockInfo.symbol = slist[1];
            //switch (stockInfo.symbol.Substring(0, 2))
            //{
            //    case "60":
            //    case "30":
            //    case "00":

            //        ro.items.Add(stockInfo);
            //        break;
            //    default: break;

            //}




            return ro;

        }









        /// <summary>
        /// 将获取的信息写入到数据库中,并行执行
        /// </summary>
        /// <param name="ro"></param>
        private void WriteToNamesDbParalle(ReturnInfo ro)
        {

            try
            {

                int progressCnt = 0;

                int cnt = ro.items.Count;


                IsRunning = true;

                Progress = 0;


                var po = new ParallelOptions()
                {
                    CancellationToken = cts.Token,
                    MaxDegreeOfParallelism = MaxThreadNum,
                };


                //foreach(var item in ro.items)
                System.Threading.Tasks.Parallel.ForEach<StockInfo>(ro.items, po, item =>
              {
                  using (var scope = _serviceScopeFactory.CreateScope())
                  {
                      var scopedServices = scope.ServiceProvider;
                      var db = scopedServices.GetRequiredService<BlogContext>();




                      string stockId = item.symbol.Trim();

                      //将新浪的名字改为网易的名字
                      string newEaseStockId = "";
                      string stockNum = stockId;
                      if (stockId.StartsWith("6")) //上海的
                      {
                          newEaseStockId = "0" + stockNum;
                      }
                      else
                      {
                          newEaseStockId = "1" + stockNum;
                      }


                      var itemindb = db.StockSet.SingleOrDefault(s => s.StockId == newEaseStockId);
                      if (itemindb != null)
                      {
                          //更新
                          if (itemindb.StockName != item.name)
                          {
                              itemindb.StockName = item.name;

                          }
                      }
                      else
                      {
                          //插入
                          itemindb = new Stock();

                          itemindb.StockId = newEaseStockId;
                          itemindb.StockName = item.name;
                          db.StockSet.Add(itemindb);
                      }

                      //在foreach 中不会等等
                      db.SaveChanges();

                      Interlocked.Increment(ref progressCnt);
                      //增加1%才更新
                      float progress = (float)progressCnt / cnt;
                      if (progress - 0.05f >= Progress || progress == 1)
                      {
                          Progress = progress;
                      }

                  }

              }
                );
            }
            finally
            {
                IsRunning = false;
            }


        }


    }
}
