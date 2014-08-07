using iAFWebHost.Entities;
using iAFWebHost.Services;
using iAFWebHost.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAFWebHost.Tests.Services
{
    [TestClass]
    public class EmailServiceTests
    {
        [TestMethod]
        public void Email_Test_UpsertData()
        {
            EmailService service = new EmailService();
            for (int i = 0; i < 5; i++)
            {
                Email input = GenerateEmailEntity();
                Email output = service.Upsert(input);
                Assert.IsNotNull(output);
            }

            for (int i = 0; i < 15; i++)
            {
                Email input = GenerateEmailEntity("yuri@i.af");
                Email output = service.Upsert(input);
                Assert.IsNotNull(output);
            }
        }

        [TestMethod]
        public void Email_Test_GetEmails()
        {
            EmailService service = new EmailService();
            var output = service.GetEmails(0,10,0,null,null,null,null,"desc");
        }

        [TestMethod]
        public void Email_Test_GetInboxEmails()
        {
            string email = GetTestRecepientEmail().MD5();
            EmailService service = new EmailService();
            Dto<Email> emails = service.GetInboxEmails(email, 0, 10, 0);
            int count = emails.TotalRows;
            int serviceCount = service.CountInboxEmails(email, 0, 10, 0);
        }

        [TestMethod()]
        public void Mailbox_Test_Upsert()
        {
            EmailService service = new EmailService();
            var mailbox = GenerateMailboxEntity();
            var response = service.Upsert(mailbox);
            Assert.IsNotNull(response);
        }

        [TestMethod()]
        public void Mailbox_Test_PopulateMailbox()
        {
            EmailService EmailService = new EmailService();
            var mailbox = GenerateMailboxEntity();
            var response = EmailService.Upsert(mailbox);

            EmailService emailService = new EmailService();
            for (int i = 0; i < 10; i++)
            {
                Email input = GenerateEmailEntity();
                Email output = emailService.Upsert(input);
                Assert.IsNotNull(output);
            }

            Assert.IsNotNull(response);
        }

        private Email GenerateEmailEntity(string recepientEmail = null)
        {
            Email email = new Email();
            email.Token = Path.GetRandomFileName();
            if (String.IsNullOrEmpty(recepientEmail))
                email.RecipientEmail = GetTestRecepientEmail();
            else
                email.RecipientEmail = recepientEmail;

            email.MailboxId = email.RecipientEmail.MD5();
            email.From = Path.GetRandomFileName() + "@i.af";
            email.FromEmail = Path.GetRandomFileName() + "@i.af";
            email.Subject = "Subject " + Path.GetRandomFileName();
            email.BodyHtml = "<h1>This is an H1 tag example</h1>" + " This is a sample body " + Path.GetRandomFileName() + "<h2>This is a very long line, This is a very long line This is a very long line This is a very long line, This is a very long line, This is a very long line, This is a very long line, This is a very long line, This is a very long line, This is a very long line, This is a very long line, This is a very long line, This is a very long line, This is a very long line, </h2>";
            email.BodyPlain = "This is a sample body" + Path.GetRandomFileName();
            email.TimeStamp = DateTime.UtcNow;
            return email;
        }

        private Mailbox GenerateMailboxEntity()
        {
            Mailbox mailbox = new Mailbox();
            mailbox.RecepientEmail = GetTestRecepientEmail();
            mailbox.TimeStamp = DateTime.UtcNow;
            mailbox.Id = mailbox.RecepientEmail.MD5();
            return mailbox;
        }

        private string GetTestRecepientEmail()
        {
            return "demo@i.af";
        }
    }
}
