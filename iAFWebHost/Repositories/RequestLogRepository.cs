using Couchbase;
using iAFWebHost.Entities;
using iAFWebHost.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iAFWebHost.Repositories
{
    public class RequestLogRepository : UrlClientRepositoryBase<RequestLog>
    {
        public Dto<RequestLog> GetRequestsWithReferrals(int page = 0, int limit = 10, int skip = 0, string startKey = null, string endKey = null, string startDocId = null, string endDocId = null, string sort = null)
        {
            return GetDto("requestlog_referral", StaleMode.False, page, limit, skip, false, 0, false, startKey, endKey, startDocId, endDocId, sort);
        }

        public Dto<RequestLog> GetRequests(int page = 0, int limit = 10, int skip = 0, string startKey = null, string endKey = null, string startDocId = null, string endDocId = null, string sort = null)
        {
            return GetDto("requestlog_list", StaleMode.False, page, limit, skip, false, 0, false, startKey, endKey, startDocId, endDocId, sort);
        }
    }
}