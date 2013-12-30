using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iAFWebHost.Utils;

namespace iAFWebHost.Entities
{
    /// <summary>
    /// Database entity
    /// </summary>
    public class Url : EntityBase
    {
        public Url()
        {
            Users = new List<string>();
            Tags = new List<string>();
            UtcDate = DateTime.UtcNow;
        }

        /// <summary>
        /// ShortId is a Base58 Encoding of the ulong Id integer.
        /// </summary>
        /// <value>
        /// ShortId
        /// </value>
        [JsonProperty("S")]
        public string ShortId
        {
            get
            {
                ulong longId;
                if (!String.IsNullOrEmpty(Id) && ulong.TryParse(Id, out longId))
                    return longId.EncodeBase58();
                else
                    return String.Empty;
            }
        }

        [JsonProperty("Users")]
        public List<string> Users { get; set; }

        [JsonProperty("Tags")]
        public List<string> Tags { get; set; }

        /// <summary>
        /// Gets the host of the supplied URL variable base on Uri.TryCreate method.
        /// Return an empty string if we are unable to parse our host value.
        /// </summary>
        /// <value>
        /// The host.
        /// </value>
        [JsonProperty("H")]
        public string Host
        {
            get
            {
                Uri uri;
                if (Uri.TryCreate(Href, UriKind.Absolute, out uri))
                    return uri.Host;
                else
                    return String.Empty;
            }
        }

        /// <summary>
        /// Main url that is stored in the database and used for redirection purposes.
        /// </summary>
        /// <value>
        /// The href.
        /// </value>
        [JsonProperty("U")]
        public string Href { get; set; }

        /// <summary>
        /// Gets or sets the actual href after service attempts to resolve destination Url
        /// </summary>
        /// <value>
        /// The href actual.
        /// </value>
        [JsonProperty("UA")]
        public string HrefActual { get; set; }

        /// <summary>
        /// Gets or sets the HTTP response code.Recorded during initial HEAD operation
        /// </summary>
        /// <value>
        /// The HTTP response code.
        /// </value>
        [JsonProperty("RC")]
        public int HttpResponseCode { get; set; }

        /// <summary>
        /// Gets or sets the length of the content as returned by HTTP Head operation.
        /// </summary>
        /// <value>
        /// The length of the content.
        /// </value>
        [JsonProperty("Cl")]
        public long HttpContentLength { get; set; }

        /// <summary>
        /// Gets or sets the type of the content as returned by HTTP Head operation.
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        [JsonProperty("Ct")]
        public string HttpContentType { get; set; }

        [JsonProperty("Ri")]
        public string HttpResponseIP { get; set; }

        /// <summary>
        /// Gets or sets the HTTP time stamp. Stores the date and time of the initial HTTP HEAD request.
        /// </summary>
        /// <value>
        /// The HTTP time stamp.
        /// </value>
        [JsonProperty("TS")]
        public DateTime HttpTimeStamp { get; set; }

        [JsonProperty("Tl")]
        public string Title { get; set; }

        [JsonProperty("Sm")]
        public string Summary { get; set; }

        /// <summary>
        /// Represent a dynamic (not stored in the database) absolute Url value 
        /// based on a shortId and a root system domain http://i.af
        /// </summary>
        /// <value>
        /// The short href.
        /// </value>
        [JsonIgnore]
        public string ShortHref
        {
            get
            {
                return String.Format("http://i.af/{0}", ShortId);
            }
        }

        /// <summary>
        /// Gets or sets the flag. Internal record status.
        /// Possible Flag values are: 
        /// 0 = new record in inactive state (Inactive)
        /// 1 = retrieved from database (Enabled).
        /// </summary>
        /// <value>
        /// The flag.
        /// </value>
        [JsonProperty("F")]
        public int Flag { get; set; }

        /// <summary>
        /// Gets the type of the object in a human readable format.
        /// Used internally to identify json document type in the couchbase database cluster.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [JsonProperty("T")]
        public override string Type
        {
            get
            {
                return "url";
            }
        }

        [JsonProperty(PropertyName = "D")]
        public DateTime UtcDate { get; set; }
    }
}