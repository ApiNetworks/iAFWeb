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
        public virtual string Id { get; set; }

        [JsonIgnore]
        public virtual string ViewKey { get; set; }

        [JsonIgnore]
        public virtual ulong CasValue { get; set; }

        [JsonProperty("type")]
        public abstract string Type { get; } 
    }
}