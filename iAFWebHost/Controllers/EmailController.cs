using iAFWebHost.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iAFWebHost.Utils;
using System.Security.Cryptography;
using System.Text;
using iAFWebHost.Services;
using iAFWebHost.Models;

namespace iAFWebHost.Controllers
{
    public class EmailController : Controller
    {
        private EmailService emailService = null;

        public EmailController()
        {
            emailService = new EmailService();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var inboxModel = new InboxConfigModel();
            inboxModel.InboxEmail = @System.IO.Path.GetRandomFileName() + "@i.af";
            return View(inboxModel);
        }

        [HttpPost]
        public ActionResult Index(InboxConfigModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToRoute("Inbox", new { id = model.InboxId, email = model.InboxEmail });
            }

            return View(model);
        }

        public ActionResult Inbox(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                InboxModel model = new InboxModel();
                var mailbox = emailService.GetEmails(id);
                if (mailbox != null)
                {
                    model.RecepientEmail = mailbox.RecepientEmail;
                    model.MailboxId = mailbox.Id;
                    model.Emails = mailbox.Emails;
                    model.TotalEmails = mailbox.TotalEmails;
                    return View(model);
                }
                else
                {
                    if (!String.IsNullOrEmpty(Request["email"]))
                        model.RecepientEmail = Request["email"];

                    return View("MailboxNotFound", model);
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult CatchAll()
        {
            InboxModel model = new InboxModel();
            var mailbox = emailService.GetAll(0, 100, 0);
            if (mailbox != null)
            {
                model.RecepientEmail = mailbox.RecepientEmail;
                model.MailboxId = mailbox.Id;
                model.Emails = mailbox.Emails;
                model.TotalEmails = mailbox.TotalEmails;
                return View("Inbox", model);
            }
            else
            {
                return View("MailboxNotFound", model);
            }
        }

        public ActionResult Message(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var email = emailService.Get(id);
                if (email != null)
                {
                    EmailModel model = new EmailModel();
                    model.EmailMessage = email;
                    return View(model);
                }
            }

            return View("MessageNotFound");
        }
    }
}
