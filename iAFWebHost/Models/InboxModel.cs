using iAFWebHost.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iAFWebHost.Models
{
    public class InboxModel
    {
        public InboxModel()
        {
            Pager = new PageHelper();
        }
        
        public PageHelper Pager { get; set; }
        public List<Email> Emails { get; set; }
        public int TotalEmails { get; set; }
        public string MailboxId { get; set; }
        public string RecepientEmail { get; set; }
    }
}