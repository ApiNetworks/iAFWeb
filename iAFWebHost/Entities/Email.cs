using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using iAFWebHost.Utils;

namespace iAFWebHost.Entities
{
    public class Email : EntityBase
    {
        [Required]
        public string MailboxId { get; set; }
        public string From { get; set; }
        [Required]
        public string FromEmail { get; set; }
        [Required]
        public string RecipientEmail { get; set; }
        public string Subject { get; set; }

        [JsonIgnore]
        public string SubjectPreview { get { return Subject.CharacterLimit(30); } }

        public string BodyPlain { get; set; }

        [JsonIgnore]
        public string BodyPreview { get { return BodyPlain.CharacterLimit(30);} }
        public string StrippedText { get; set; }
        public string StrippedSignature { get; set; }
        public string BodyHtml { get; set; }
        public string StrippedHtml { get; set; }
        public int AttachmentCount { get; set; }
        public string Attachment { get; set; }
        public string Token { get; set; }
        public string Signature { get; set; }
        public string Headers { get; set; }
        public string ContentId { get; set; }
        public long TimeStampSeconds { get; set; }
        public DateTime TimeStamp { get; set; }

        [JsonProperty("T")]
        public override string Type
        {
            get
            {
                return "email";
            }
        }
    }
}