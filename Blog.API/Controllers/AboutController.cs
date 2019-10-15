using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Blog.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutController : ControllerBase
    {
        private readonly BlogContext _db;
        public AboutController(BlogContext db)
        {
            _db = db;
        }


        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("ServerVersion")]
        [Produces("application/json")]
        public ActionResult GetServerVersion()
        {
            var temp2 = Assembly.GetEntryAssembly();
            string ret = temp2.GetName().Version.ToString();
            return Ok(ret);
        }




        /// <summary>
        /// 获取数据库中数据时间的信息。
        /// </summary>
        /// <returns></returns>
        [HttpGet("DateInfo")]
        public async Task<ActionResult> GetDateInfo()
        {
            var dateGongGao = await (from i in _db.SharingSet
                                     orderby i.DateGongGao descending
                                     select i.DateGongGao
                               ).FirstOrDefaultAsync();

            var realTimeItem = await (from i in _db.RealTimeDataSet
                                      where i.HuanShouLiu != null
                                      orderby i.Date descending
                                      select new { i.Date, i.StockId }
                               ).FirstOrDefaultAsync();

            DateTime dateDaily = default;
            if (realTimeItem != null)
            {
                dateDaily = await (from i in _db.DayDataSet
                                   where i.StockId == realTimeItem.StockId
                                   orderby i.Date descending
                                   select i.Date
                       ).FirstOrDefaultAsync();
            }
            else
            {
                //没有实时数据时，才会直接从日线数据中查询
                dateDaily = await (from i in _db.DayDataSet
                                   orderby i.Date descending
                                   select i.Date
                          ).FirstOrDefaultAsync();
            }

            object obj = new
            {
                GongGao = dateGongGao,
                Daily = dateDaily,
                RealTime = realTimeItem != null ? realTimeItem.Date : default,
            };

            return Ok(obj);
        }


        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
