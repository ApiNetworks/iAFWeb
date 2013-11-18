using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iAFWebHost.Models
{
    public class ErrorModel
    {
        public string Id { get; set; }
        public int SequenceID { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Level { get; set; }
        public bool HasStackTrace { get; set; }
        public int UserStackFrameNumber { get; set; }
        public string LoggerName { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public string FormattedMessage { get; set; }
        public IDictionary<object, object> Properties { get; set; }
        public string Type
        {
            get { return "error"; }
        }
    }
}