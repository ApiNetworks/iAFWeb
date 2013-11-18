using Couchbase;
using Couchbase.Extensions;
using NLog;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAF.NLog
{
    [Target("Couchbase")]
    public sealed class CouchbaseTarget : Target
    {
        private static CouchbaseClient _client = null;

        static CouchbaseTarget()
        {
            _client = new CouchbaseClient("couchbaseLog");
        }

        protected override void Write(LogEventInfo logEvent)
        {
            Error error = new Error(logEvent);
            var result = _client.ExecuteStoreJson(Enyim.Caching.Memcached.StoreMode.Add, error.Id, error);
        }
    }
}
