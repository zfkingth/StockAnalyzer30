using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Linq;


namespace Blog.Model
{
    public partial class SearchResult
    {
        public SearchResult()
        {

        

        }


        [MaxLength(32)]
        public string ActionName { get; set; }

        [MaxLength(512)]

        public string ActionParams { get; set; }


        public DateTime ActionDate { get; set; }

        [MaxLength(4096)]
        public string ActionReslut { get; set; }



    }
}
