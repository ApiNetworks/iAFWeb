using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace iAFWebHost.Models
{
    public class PageHelper
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Skip { get; set; }
        public int TotalRows { get; set; }
        public int FromRecord
        {
            get
            {
                if (Page == 1)
                    return 1;
                else
                    return (Page - 1) * PageSize;
            }
        }
        public int ToRecord
        {
            get
            {
                int r = Page * PageSize;
                if (r > TotalRows)
                    return TotalRows;
                else
                    return r;
            }
        }
        public string PreviousKey { get; set; }
        public string PreviousKeyId { get; set; }
        public string NextKey { get; set; }
        public string NextKeyId { get; set; }
        public string TempKey { get; set; }
        public string TempKeyId { get; set; }
        public string Sort { get; set; }

        public string NextPath()
        {
            StringBuilder sb = new StringBuilder();
            if (Page * PageSize >= TotalRows)
                sb.AppendFormat("p={0}&ps={1}", 1, PageSize);
            else
            {
                sb.AppendFormat("p={0}&ps={1}", Page + 1, PageSize);
                if (!String.IsNullOrEmpty(NextKey) && !String.IsNullOrEmpty(NextKeyId))
                    sb.AppendFormat("&nk={0}&nid={1}", NextKey, NextKeyId);
                if (!String.IsNullOrEmpty(PreviousKey) && !String.IsNullOrEmpty(PreviousKeyId))
                    sb.AppendFormat("&pk={0}&pid={1}", PreviousKey, PreviousKeyId);
            }
            return sb.ToString();
        }

        public string PrevPath()
        {
            int page = Page - 1;
            if (page < 1) page = 1;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("p={0}&ps={1}", page, PageSize);
            if (!String.IsNullOrEmpty(TempKey) && !String.IsNullOrEmpty(TempKeyId))
                sb.AppendFormat("&nk={0}&nid={1}", TempKey, TempKeyId);
            return sb.ToString();
        }
    }
}