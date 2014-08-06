using Couchbase;
using Couchbase.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;

namespace iAFWebHost.Repositories
{
    public static class CouchbaseManager
    {
        //static readonly LazyThreadSafetyMode threadSafetyMode = LazyThreadSafetyMode.PublicationOnly;
        
        //private static readonly Lazy<CouchbaseClient> client =
        //    new Lazy<CouchbaseClient>(() => new CouchbaseClient());

        private static readonly Lazy<CouchbaseClient> emailClient =
            new Lazy<CouchbaseClient>(() => new CouchbaseClient((CouchbaseClientSection)ConfigurationManager.GetSection("couchbaseEmailbucket")));

        private static readonly Lazy<CouchbaseClient> urlClient =
            new Lazy<CouchbaseClient>(() => new CouchbaseClient((CouchbaseClientSection)ConfigurationManager.GetSection("couchbaseUrlbucket")));

        public static CouchbaseClient UrlInstance { get { return urlClient.Value; } }
        public static CouchbaseClient EmailInstance { get { return emailClient.Value; } }
    }
}