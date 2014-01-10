using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iAFWebHost.Models
{
    public class RequestLogModel
    {
        public string Id { get; set; }
        public string RemoteIP { get; set; }
        public string RequestUrl { get; set; }
        public string Referrer { get; set; }
        public string Raw { get; set; }
        public DateTime DT { get; set; }
        public string Type
        {
            get { return "log"; }
        }
    }
}