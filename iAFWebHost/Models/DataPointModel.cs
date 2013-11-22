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
        public long Sum { get; set; }
        public long Min { get; set; }
        public long Max { get; set; }
        public long Count { get; set; }
        public long SumSqr { get; set; }
    }
}