using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace iAFWebHost.Models
{
    public class PageModel
    {
        public PageModel()
        {
            Urls = new List<UrlModel>();
            Pager = new PageHelper();
            Requests = new List<RequestLogModel>();
        }

        public int UrlCount { get; set; }
        public List<UrlModel> Urls { get; set; }
        public List<ErrorModel> Errors { get; set; }
        public List<RequestLogModel> Requests { get; set; }
        public List<DataPointModel> HourlyDataPoints { get; set; }
        public List<DataPointModel> DailyDataPoints { get; set; }
        public List<DataPointModel> MonthlyDataPoints { get; set; }
        public UrlModel UrlModel { get; set; }
        public PageHelper Pager { get; set; }
        public string UserName { get; set; }
        public string Host { get; set; }
        public string Tag { get; set; }
    }
}