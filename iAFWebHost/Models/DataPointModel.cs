using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iAFWebHost.Models
{
    public class DataPointModel
    {
        public string ShortId { get; set; }
        public DateTime TimeStamp { get; set; }
        public ulong Value { get; set; }
    }
}