using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iAFWebHost.Models
{
    public class PageModel
    {
        public PageModel()
        {
            Urls = new List<UrlModel>();
        }

        public int UrlCount { get; set; }
        public string UserName { get; set; }
        public List<UrlModel> Urls { get; set; }
        public UrlModel UrlModel { get; set; }
    }
}