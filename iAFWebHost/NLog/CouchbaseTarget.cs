using Couchbase;
using Couchbase.Extensions;
using iAFWebHost.Entities;
using iAFWebHost.Repositories;
using iAFWebHost.Services;
using NLog;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iAFWebHost.NLog
{
    [Target("Couchbase")]
    public sealed class CouchbaseTarget : Target
    {
        protected override void Write(LogEventInfo logEvent)
        {
            try
            {
                Error error = new Error(logEvent);
                CouchbaseManager.UrlInstance.ExecuteStoreJson(Enyim.Caching.Memcached.StoreMode.Add, error.Id, error);
            }
            catch(Exception ex)
            {

            }
        }
    }
}