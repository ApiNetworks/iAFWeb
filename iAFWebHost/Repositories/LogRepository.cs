using Couchbase;
using iAFWebHost.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iAFWebHost.Repositories
{
    public partial class LogRepository : RepositoryBase<Error>
    {
        public Dto<Error> GetErrors(int page = 0, int limit = 10, int skip = 0, string startKey = null, string endKey = null, string startDocId = null, string endDocId = null, string sort = null)
        {
            return GetDto("error_list", StaleMode.AllowStale, page, limit, skip, false, 0, false, startKey, endKey, startDocId, endDocId, sort);
        }
    }
}