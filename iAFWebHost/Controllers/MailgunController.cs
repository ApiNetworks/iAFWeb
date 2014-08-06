using iAFWebHost.Entities;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using iAFWebHost.Utils;
using iAFWebHost.Services;

namespace iAFWebHost.Controllers
{
    public class MailgunController : BaseController
    {
        private EmailService emailService = null;

        public MailgunController()
        {
            emailService = new EmailService();
        }

        // POST: /MailgunUpload/Mail
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Gateway(FormCollection form)
        {
            try
            {
                Email email = new Email();

                //recipient of the message as reported by MAIL TO during SMTP chat.
                if(!String.IsNullOrEmpty(Request.Unvalidated.Form["recipient"]))
                    email.RecipientEmail = Request.Unvalidated.Form["recipient"].ToLower();

                if (!String.IsNullOrEmpty(Request.Unvalidated.Form["recipient"]))
                    email.MailboxId = email.RecipientEmail.MD5();

                //sender of the message as reported by MAIL FROM during SMTP chat. Note: this value may differ from From MIME header.
                email.FromEmail = Request.Unvalidated.Form["sender"];
                //sender of the message as reported by From message header, for example “Bob <bob@example.com>”.
                email.From = Request.Unvalidated.Form["from"];
                //subject string.
                email.Subject = Request.Unvalidated.Form["subject"];
                //text version of the email. This field is always present. If the incoming message only has HTML body, Mailgun will create a text representation for you.
                email.BodyPlain = Request.Unvalidated.Form["body-plain"];
                //text version of the message without quoted parts and signature block (if found).
                email.StrippedText = Request.Unvalidated.Form["stripped-text"];
                //the signature block stripped from the plain text message (if found).
                email.StrippedSignature = Request.Unvalidated.Form["stripped-signature"];
                // 	HTML version of the message, if message was multipart. Note that all parts of the message will be posted, not just text/html. For instance if a message arrives with “foo” part it will be posted as “body-foo”.
                email.BodyHtml = Request.Unvalidated.Form["body-html"];
                //HTML version of the message, without quoted parts.
                email.StrippedHtml = Request.Unvalidated.Form["stripped-html"];
                //how many attachments the message has.
                email.AttachmentCount = Request.Unvalidated.Form["attachment-count"].TryParseInt();
                //attached file (‘x’ stands for number of the attachment). Attachments are handled as file uploads, encoded as multipart/form-data.
                email.Attachment = Request.Unvalidated.Form["attachment-x"];
                //number of second passed since January 1, 1970 (see securing webhooks).
                email.TimeStampSeconds = Request.Unvalidated.Form["timestamp"].TryParseLong();
                //randomly generated string with length 50 (see securing webhooks).
                email.Token = Request.Unvalidated.Form["token"];
                //string with hexadecimal digits generate by HMAC algorithm (see securing webhooks).
                email.Signature = Request.Unvalidated.Form["signature"];
                //list of all MIME headers dumped to a json string (order of headers preserved).
                email.Headers = Request.Unvalidated.Form["message-headers"];
                //JSON-encoded dictionary which maps Content-ID (CID) of each attachment to the corresponding attachment-x parameter. This allows you to map posted attachments to tags like <img src='cid'> in the message body.
                email.ContentId = Request.Unvalidated.Form["content-id-map"];

                email.TimeStamp = FromUnixTime(email.TimeStampSeconds);

                // verify message
                if (Verify("key-6huijczyq8rkjl8a5bndzwf6y523isx7", email.Token, email.TimeStampSeconds.ToString(), email.Signature))
                {
                    emailService.Upsert(email);

                    Mailbox mailbox = new Mailbox();
                    mailbox.RecepientEmail = email.RecipientEmail;
                    mailbox.Id = email.RecipientEmail.MD5();
                    mailbox.TimeStamp = DateTime.UtcNow;

                    emailService.Upsert(mailbox);
                }

                //emailService.Upsert(email);

                // Work your magic
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Content("ok");
        }

        private static DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        public static bool Verify(string apikey, string token, string timestamp, string signature)
        {
            var hmac = new HMACSHA256(Encoding.ASCII.GetBytes(apikey));
            var sigBytes = hmac.ComputeHash(Encoding.ASCII.GetBytes(timestamp + token));
            string sigString = BitConverter.ToString(sigBytes).Replace("-", "");
            return signature.Equals(sigString, StringComparison.OrdinalIgnoreCase);
        }
    }
}