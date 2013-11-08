using Couchbase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iAFWebHost.Services
{
    /// <summary>
    /// Base service class for all CouchBase operations and service calls
    /// </summary>
    public class BaseService
    {
        /// <summary>
        /// Gets the database client.
        /// </summary>
        /// <value>
        /// The database client.
        /// </value>
        protected static CouchbaseClient NoSqlClient { get; private set; }

        /// <summary>
        /// Initializes the <see cref="BaseService"/> class.
        /// </summary>
        static BaseService()
        {
            NoSqlClient = new CouchbaseClient();
        }

        public ulong Increment()
        {
            return NoSqlClient.Increment("url::count", 1, 1);
        }
    }
}