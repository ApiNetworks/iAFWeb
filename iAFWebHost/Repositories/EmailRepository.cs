using iAFWebHost.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iAFWebHost.Utils;
using Couchbase;

namespace iAFWebHost.Repositories
{
    public class EmailRepository : EmailClientRepositoryBase<Email>
    {
        public Email Upsert(Email email)
        {
            string hashKey = (email.Token + email.TimeStampSeconds.ToString()).MD5();
            if (!String.IsNullOrEmpty(hashKey))
            {
                email.Id = String.Format("{0}_{1}", email.RecipientEmail.MD5(), hashKey);
                ulong CasResult = base.Save(email, TimeSpan.FromHours(24));
            }
            return email;
        }

        public Dto<Email> GetInboxEmails(string inbox, int page = 0, int limit = 10, int skip = 0)
        {
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MaxValue;

            object[] startKey = { inbox, startDate.Year, startDate.Month, startDate.Day, startDate.Hour, startDate.Minute, startDate.Second };
            object[] endKey = { inbox + "\u0fff", endDate.Year, endDate.Month, endDate.Day, endDate.Hour, endDate.Minute, endDate.Second };

            return GetInboxEmails(page, limit, skip, startKey, endKey, null, null, null);
        }

        public int CountInboxEmails(string inbox)
        {
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MaxValue;

            object[] startKey = { inbox, startDate.Year, startDate.Month, startDate.Day, startDate.Hour, startDate.Minute, startDate.Second };
            object[] endKey = { inbox + "\u0fff", endDate.Year, endDate.Month, endDate.Day, endDate.Hour, endDate.Minute, endDate.Second };

            return CountInboxEmails(0, 10, 0, startKey, endKey, null, null, null);
        }

        public int CountInboxEmails(int page = 0, int limit = 10, int skip = 0, object[] startKey = null, object[] endKey = null, string startDocId = null, string endDocId = null, string sort = null)
        {
            return Count("email_list", StaleMode.False, false, 0, true, startKey, endKey, startDocId, endDocId);
        }

        public Dto<Email> GetInboxEmails(int page = 0, int limit = 10, int skip = 0, object[] startKey = null, object[] endKey = null, string startDocId = null, string endDocId = null, string sort = null)
        {
            return GetDto("email_list", StaleMode.False, page, limit, skip, false, 0, false, startKey, endKey, startDocId, endDocId, sort);
        }

        public Dto<Email> GetEmailList(int page = 0, int limit = 10, int skip = 0, string startKey = null, string endKey = null, string startDocId = null, string endDocId = null, string sort = null)
        {
            return GetDto("email_list", StaleMode.False, page, limit, skip, false, 0, false, startKey, endKey, startDocId, endDocId, sort);
        }
    }
}