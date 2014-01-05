using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iAFWebHost.Entities
{
    [Serializable]
    public class RequestLog : EntityBase
    {
        [JsonProperty("IP")]
        public string RemoteIP { get; set; }
        [JsonProperty("RUrl")]
        public string RequestUrl { get; set; }
        [JsonProperty("Ref")]
        public string Referrer { get; set; }
        [JsonProperty("Raw")]
        public string Raw { get; set; }
        [JsonProperty("DT")]
        public DateTime DT { get; set; }
        public override string Type
        {
            get { return "log"; }
        }
    }
}