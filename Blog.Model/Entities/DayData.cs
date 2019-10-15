using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Blog.Model
{
    public partial class DayData
    {


        [MaxLength(10)]
        public string StockId { get; set; }
        public DateTime Date { get; set; }

        public float Open { get; set; }
        public float Low { get; set; }
        public float High { get; set; }
        public float Close { get; set; }
        /// <summary>
        /// �ɽ���
        /// </summary>
        public float Volume { get; set; }


        /// <summary>
        /// �ɽ����
        /// </summary>
        public float Amount { get; set; }

        /// <summary>
        /// �ǵ���
        /// </summary>
        public float? ZhangDieFu { get; set; }
        /// <summary>
        /// ����ֵ
        /// </summary>
        public float? ZongShiZhi { get; set; }

        /// <summary>
        /// ��ͨ��ֵ
        /// </summary>
        public float? LiuTongShiZhi { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public float? HuanShouLiu { get; set; }

        public DayDataType Type { get; set; }

        [ForeignKey("StockId")]

        public virtual Stock Stock { get; set; }

        
    }
}
