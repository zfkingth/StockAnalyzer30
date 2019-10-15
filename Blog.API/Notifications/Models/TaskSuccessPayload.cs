using Blog.API.ViewModels;
using Blog.Model;
using System.Collections.Generic;

namespace Blog.API.Notifications.Models
{
    public class TaskSuccessPayload
    {
        public string Message { get; set; }
        public List<RealTimeData> ResultList { get; set; }
    }
}