using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iAFWebHost.Entities
{
    public class StatRecord
    {
        public long Sum { get; set; }
        public long Min { get; set; }
        public long Max { get; set; }
        public long Count { get; set; }
        public long SumSqr { get; set; }
    }
}