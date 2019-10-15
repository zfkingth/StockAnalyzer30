using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Linq;


namespace Blog.Model
{
    public partial class Stock
    {
        public Stock()
        {

            this.DayDataSet = new List<DayData>();

            this.SharingSet = new List<Sharing>();

            this.RealTimeDataSet = new List<RealTimeData>();

            this.StockNumSet = new List<StockNum>();

        }


        [Key]
        [MaxLength(10)]
        public string StockId { get; set; }

        [MaxLength(10)]

        public string StockName { get; set; }


        public DateTime RealDataUpdated { get; set; }

        public StockTypeEnum StockType { get; set; } = StockTypeEnum.Stock;



        public virtual ICollection<DayData> DayDataSet { get; set; }

        /// <summary>
        /// 分红，送股信息
        /// </summary>
        public virtual ICollection<Sharing> SharingSet { get; set; }
        public virtual ICollection<RealTimeData> RealTimeDataSet { get; set; }



        public virtual ICollection<StockNum> StockNumSet { get; set; }
    }
}
