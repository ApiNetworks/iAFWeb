﻿using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;

namespace iAF.NLog
{
    [Serializable]
    public class Error
    {
        public Error(LogEventInfo logEventInfo)
        {
            this.Id = "error_" + Guid.NewGuid().ToString();
            this.SequenceID = logEventInfo.SequenceID;
            this.TimeStamp = logEventInfo.TimeStamp;
            this.Level = logEventInfo.Level.ToString();
            this.HasStackTrace = logEventInfo.HasStackTrace;

            this.UserStackFrame = logEventInfo.UserStackFrame;
            this.UserStackFrameNumber = logEventInfo.UserStackFrameNumber;
            this.StackTrace = logEventInfo.StackTrace;
            this.Exception = logEventInfo.Exception;
            this.LoggerName = logEventInfo.LoggerName;

            this.Message = logEventInfo.Message;
            this.Parameters = logEventInfo.Parameters;
            this.FormattedMessage = logEventInfo.FormattedMessage;
            this.Properties = logEventInfo.Properties;
        }

        public string Id { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "Backwards compatibility", MessageId = "ID")]
        public int SequenceID { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", Justification = "Backwards compatibility.", MessageId = "TimeStamp")]
        public DateTime TimeStamp { get; set; }

        public string Level { get; set; }
        public bool HasStackTrace { get; set; }
        public StackFrame UserStackFrame { get; set; }
        public int UserStackFrameNumber { get; set; }
        public StackTrace StackTrace { get; set; }
        public Exception Exception { get; set; }
        public string LoggerName { get; set; }
        public string Message { get; set; }

        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "For backwards compatibility.")]
        public object[] Parameters { get; set; }

        public string FormattedMessage { get; set; }
        public IDictionary<object, object> Properties { get; set; }

        [JsonProperty("T")]
        public string Type
        {
            get { return "error"; }
        }
    }
}