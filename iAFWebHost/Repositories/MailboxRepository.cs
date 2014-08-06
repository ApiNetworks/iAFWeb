using iAFWebHost.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iAFWebHost.Repositories
{
    public class MailboxRepository : EmailClientRepositoryBase<Mailbox>
    {
        public Mailbox Upsert(Mailbox mailbox)
        {
            ulong CasResult = base.Save(mailbox);
            return mailbox;
        }
    }
}