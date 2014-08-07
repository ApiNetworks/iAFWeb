using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Couchbase.Extensions;
using Enyim.Caching.Memcached;
using iAFWebHost.Entities;
using iAFWebHost.Utils;
using iAFWebHost.Repositories;
using Newtonsoft.Json;

namespace iAFWebHost.Services
{
    public class EmailService : BaseService
    {
        private EmailRepository _emailRepository;
        private MailboxRepository _mailboxRepository;

        public EmailService()
        {
            _emailRepository = new EmailRepository();
            _mailboxRepository = new MailboxRepository();
        }

        #region Mailbox
        public Mailbox GetInfo(string id)
        {
            try
            {
                return _mailboxRepository.Get(id);
            }
            catch (Exception ex)
            {
                throw HandleException(new object[] { id }, ex);
            }
        }

        public Mailbox GetEmails(string id, int page = 0, int limit = 10, int skip = 0)
        {
            try
            {
                var mailbox = _mailboxRepository.Get(id);
                if (mailbox != null)
                {
                    EmailService emailService = new EmailService();
                    var dto = emailService.GetInboxEmails(id, page, limit, skip);
                    if (dto != null && dto.Entities != null)
                    {
                        mailbox.Emails = dto.Entities;
                        mailbox.TotalEmails = dto.TotalRows;
                    }
                }
                return mailbox;
            }
            catch (Exception ex)
            {
                throw HandleException(new object[] { id }, ex);
            }
        }

        public Mailbox GetAll(int page = 0, int limit = 10, int skip = 0)
        {
            try
            {
                Mailbox mailbox = new Mailbox();
                mailbox.RecepientEmail = "catchall@i.af";
                if (mailbox != null)
                {
                    EmailService emailService = new EmailService();
                    var dto = emailService.GetEmails(page, limit, skip, null, null, null, null, "desc");
                    if (dto != null && dto.Entities != null)
                    {
                        mailbox.Emails = dto.Entities;
                        mailbox.TotalEmails = dto.TotalRows;
                    }
                }
                return mailbox;
            }
            catch (Exception ex)
            {
                throw HandleException(new object[] { page, limit, skip }, ex);
            }
        }

        public Mailbox Upsert(Mailbox mailbox)
        {
            if (mailbox == null)
                throw new ArgumentNullException("Email cannot be null");

            try
            {
                return _mailboxRepository.Upsert(mailbox);
            }
            catch (Exception ex)
            {
                throw HandleException(new object[] { mailbox }, ex);
            }
        }
        #endregion

        #region Email
        public Email Upsert(Email email)
        {
            if (email == null)
                throw new ArgumentNullException("Email cannot be null");

            try
            {
                return _emailRepository.Upsert(email);
            }
            catch (Exception ex)
            {
                throw HandleException(new object[] { email }, ex);
            }
        }

        public Email Get(string id)
        {
            try
            {
                return _emailRepository.Get(id);
            }
            catch (Exception ex)
            {
                throw HandleException(new object[] { id }, ex);
            }
        }

        public Dto<Email> GetInboxEmails(string inbox, int page = 0, int limit = 10, int skip = 0, string startKey = null, string endKey = null, string startDocId = null, string endDocId = null, string sort = null)
        {
            if (String.IsNullOrEmpty(inbox))
                throw new ArgumentNullException("inbox");

            try
            {

                var dto = _emailRepository.GetInboxEmails(inbox, page, limit, skip, startKey, endKey, startDocId, endDocId, sort);
                dto.TotalRows = _emailRepository.CountInboxEmails(inbox);
                return dto;
            }
            catch (Exception ex)
            {
                throw HandleException(new object[] { inbox, page, limit, skip }, ex);
            }
        }
        public int CountInboxEmails(string inbox, int page = 0, int limit = 10, int skip = 0, string startKey = null, string endKey = null, string startDocId = null, string endDocId = null, string sort = null)
        {
            if (String.IsNullOrEmpty(inbox))
                throw new ArgumentNullException("inbox");

            try
            {
                return _emailRepository.CountInboxEmails(inbox, page, limit, skip, startKey, endKey, startDocId, endDocId, sort);
            }
            catch (Exception ex)
            {
                throw HandleException(new object[] { inbox, page, limit, skip }, ex);
            }
        }

        public Dto<Email> GetEmails(int page = 0, int limit = 10, int skip = 0, string startKey = null, string endKey = null, string startDocumentId = null, string endDocumentId = null, string sort = null)
        {
            try
            {
                return _emailRepository.GetEmailList(page, limit, skip, startKey, endKey, startDocumentId, endDocumentId, sort);
            }
            catch (Exception ex)
            {
                throw HandleException(new object[] { page, limit, skip, startKey, endKey, startDocumentId, endDocumentId, sort }, ex);
            }
        }
        #endregion
    }
}