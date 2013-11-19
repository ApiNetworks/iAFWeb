using iAFWebHost.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iAFWebHost.Repositories
{
    public class StatsRepository : RepositoryBase<DataPoint>
    {
        public void Increment(DataPoint entity)
        {
            base.Increment(entity.BuildKey());
        }
    }
}