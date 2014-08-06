using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iAFWebHost.Entities
{
    public class Mailbox : EntityBase
    {
        [JsonIgnore]
        public List<Email> Emails { get; set; }
        public int TotalEmails { get; set; }
        public string RecepientEmail { get; set; }
        public string Username { get; set; }
        public int Status { get; set; }
        public DateTime TimeStamp { get; set; }

        [JsonProperty("T")]
        public override string Type
        {
            get { return "mailbox"; }
        }
    }
}