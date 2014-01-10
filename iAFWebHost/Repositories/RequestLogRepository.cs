using Couchbase;
using iAFWebHost.Entities;
using iAFWebHost.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iAFWebHost.Repositories
{
    public class RequestLogRepository : RepositoryBase<RequestLog>
    {
        public Dto<RequestLog> GetReferrals(int page = 0, int limit = 10, int skip = 0, string startKey = null, string endKey = null, string startDocId = null, string endDocId = null, string sort = null)
        {
            return GetDto("requestlog_list", StaleMode.AllowStale, page, limit, skip, false, 0, false, startKey, endKey, startDocId, endDocId, sort);
        }

        public List<RequestLog> GetReferrals(int count)
        {
            DateTime startDate = DateTime.UtcNow;
            DateTime endDate = DateTime.UtcNow.AddDays(-1);

            endDate = endDate.AddDays(1);
            object[] startKey = { startDate.Year, startDate.Month, startDate.Day };
            object[] endKey = { endDate.Year, endDate.Month, endDate.Day };
            return GetReferralAggregate(startKey, endKey, count, false, 0, false);
        }

        public List<RequestLog> GetReferralAggregate(object[] startKey, object[] endKey, int limit, bool group, int groupLevel, bool reduce)
        {
            List<RequestLog> requests = new List<RequestLog>();

            if (limit > 1000)
                limit = 1000;

            var view = View("stats", "requestref");
            if (startKey != null)
                view.StartKey(startKey);
            if (startKey != null)
                view.EndKey(endKey);
            if (group) view.Group(true);
            if (groupLevel > 0) view.GroupAt(groupLevel);
            view.Reduce(reduce);

            // retrieve results
            List<IViewRow> results = view.ToList();
            if (!results.IsNullOrEmpty())
            {
                foreach (var row in results)
                {
                    if (row.Info != null)
                    {
                        RequestLog request = new RequestLog();

                        List<object> list = row.Info.Values.ToList<object>();
                        if (!list.IsNullOrEmpty() && list.Count == 2)
                        {

                        }
                    }
                }
            }

            return requests;
        }
    }
}