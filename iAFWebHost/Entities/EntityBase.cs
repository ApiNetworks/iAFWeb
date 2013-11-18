using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iAFWebHost.Entities
{
    [Serializable]
    public abstract class EntityBase
    {
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        [JsonIgnore]
        public string ViewKey { get; set; }

        [JsonIgnore]
        public ulong CasValue { get; set; }

        [JsonProperty("type")]
        public abstract string Type { get; } 
    }
}