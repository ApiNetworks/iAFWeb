using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iAFWebHost.Utils;
using Newtonsoft.Json;

namespace iAFWebHost.Entities
{
    public class Dto<T> where T : EntityBase
    {
        public List<T> Entities { get; set; }
        public int TotalRows { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Sort { get; set; }
        public string SKey { get; set; }
        public string SId { get; set; }
        public string EKey { get; set; }
        public string EId { get; set; }
    }
}