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

        public Dto<Email> GetInboxEmails(string inbox, int page = 0, int limit = 10, int skip = 0, string startKey = null, string endKey = null, string startDocId = null, string endDocId = null, string sort = null)
        {
            if(String.IsNullOrEmpty(startKey))
            {
                // init new start key
                DateTime startDate = DateTime.MinValue;
                object[] _startKey = { inbox, startDate.Year, startDate.Month, startDate.Day, startDate.Hour, startDate.Minute, startDate.Second };
                
                // define parameter
                startKey = BuildKey(_startKey); 
            }

            if (endKey == null)
            {
                DateTime endDate = DateTime.MaxValue;
                //object[] _endKey = { inbox + "\u0fff", endDate.Year, endDate.Month, endDate.Day, endDate.Hour, endDate.Minute, endDate.Second };
                object[] _endKey = { inbox, endDate.Year, endDate.Month, endDate.Day, endDate.Hour, endDate.Minute, endDate.Second };

                // define parameter
                endKey = BuildKey(_endKey); 
            }

            return GetInboxEmails(page, limit, skip, startKey, endKey, startDocId, endDocId, sort);
        }
        public int CountInboxEmails(string inbox, int page = 0, int limit = 10, int skip = 0, string startKey = null, string endKey = null, string startDocId = null, string endDocId = null, string sort = null)
        {
            if (String.IsNullOrEmpty(startKey))
            {
                // init new start key
                DateTime startDate = DateTime.MinValue;
                object[] _startKey = { inbox, startDate.Year, startDate.Month, startDate.Day, startDate.Hour, startDate.Minute, startDate.Second };

                // define parameter
                startKey = BuildKey(_startKey);
            }

            if (endKey == null)
            {
                DateTime endDate = DateTime.MaxValue;
                //object[] _endKey = { inbox + "\u0fff", endDate.Year, endDate.Month, endDate.Day, endDate.Hour, endDate.Minute, endDate.Second };
                object[] _endKey = { inbox, endDate.Year, endDate.Month, endDate.Day, endDate.Hour, endDate.Minute, endDate.Second };

                // define parameter
                endKey = BuildKey(_endKey);
            }

            return CountInboxEmails(page, limit, skip, startKey, endKey, startDocId, endDocId, sort);
        }

        public Dto<Email> GetInboxEmails(int page = 0, int limit = 10, int skip = 0, string startKey = null, string endKey = null, string startDocId = null, string endDocId = null, string sort = null)
        {
            return GetDto("email_list", StaleMode.False, page, limit, skip, false, 0, false, startKey, endKey, startDocId, endDocId, sort);
        }
        public int CountInboxEmails(int page = 0, int limit = 10, int skip = 0, string startKey = null, string endKey = null, string startDocId = null, string endDocId = null, string sort = null)
        {
            return Count("email_list", StaleMode.False, false, 0, true, startKey, endKey, startDocId, endDocId);
        }

        public Dto<Email> GetEmailList(int page = 0, int limit = 10, int skip = 0, string startKey = null, string endKey = null, string startDocId = null, string endDocId = null, string sort = null)
        {
            return GetDto("email_list", StaleMode.False, page, limit, skip, false, 0, false, startKey, endKey, startDocId, endDocId, sort);
        }
    }
}