using Couchbase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iAFWebHost.Repositories
{
    public static class CouchbaseManager
    {
        private static readonly Lazy<CouchbaseClient> client =
            new Lazy<CouchbaseClient>(() => new CouchbaseClient());

        public static CouchbaseClient Instance { get { return client.Value; } }
    }
}