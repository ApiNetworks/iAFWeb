using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iAF.Entities
{
    public class Url
    {
        public Url()
        {
            Users = new List<string>();
            Tags = new List<string>();
            UtcDate = DateTime.UtcNow;
        }

        public string Id { get; set; }

        /// <summary>
        /// ShortId is a Base58 Encoding of the ulong Id integer.
        /// </summary>
        /// <value>
        /// ShortId
        /// </value>
        public string ShortId { get; set; }

        public List<string> Users { get; set; }

        public List<string> Tags { get; set; }

        /// <summary>
        /// Gets the host of the supplied URL variable base on Uri.TryCreate method.
        /// Return an empty string if we are unable to parse our host value.
        /// </summary>
        /// <value>
        /// The host.
        /// </value>
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
        public string Href { get; set; }

        public string HrefActual { get; set; }

        /// <summary>
        /// Gets or sets the HTTP response code.Recorded during initial HEAD operation
        /// </summary>
        /// <value>
        /// The HTTP response code.
        /// </value>
        public int HttpResponseCode { get; set; }

        /// <summary>
        /// Gets or sets the length of the content as returned by HTTP Head operation.
        /// </summary>
        /// <value>
        /// The length of the content.
        /// </value>
        public long HttpContentLength { get; set; }

        /// <summary>
        /// Gets or sets the type of the content as returned by HTTP Head operation.
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        public string HttpContentType { get; set; }

        /// <summary>
        /// Gets or sets the HTTP response ip.
        /// </summary>
        /// <value>
        /// The HTTP response ip.
        /// </value>
        public string HttpResponseIP { get; set; }

        /// <summary>
        /// Gets or sets the HTTP time stamp. Stores the date and time of the initial HTTP HEAD request.
        /// </summary>
        /// <value>
        /// The HTTP time stamp.
        /// </value>
        public DateTime HttpTimeStamp { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        /// <summary>
        /// Represent a dynamic (not stored in the database) absolute Url value 
        /// based on a shortId and a root system domain http://i.af
        /// </summary>
        /// <value>
        /// The short href.
        /// </value>
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
        public int Flag { get; set; }

        public DateTime UtcDate { get; set; }

        /// <summary>
        /// Gets the type of the object in a human readable format.
        /// Used internally to identify json document type in the couchbase database cluster.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type
        {
            get
            {
                return "url";
            }
        }
    }
}