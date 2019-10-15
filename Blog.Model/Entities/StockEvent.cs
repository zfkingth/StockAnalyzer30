using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Linq;


namespace Blog.Model
{
    public partial class StockEvent
    {
        public StockEvent()
        {



        }


        [Key]
        [MaxLength(30)]
        public string EventName { get; set; }



        public DateTime LastAriseStartDate { get; set; }



        public DateTime? LastAriseEndDate { get; set; } = null;


        public EventStatusEnum Status { get; set; } = EventStatusEnum.Idle;



    }
}
