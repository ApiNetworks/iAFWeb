using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iAF.Entities
{
    [Route("/url")]
    public class UrlRequest : IReturn<UrlResponse> 
    {
        public string ShortId { get; set; }
        public string Url { get; set; }
    }
}