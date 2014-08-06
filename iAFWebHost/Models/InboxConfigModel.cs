using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using iAFWebHost.Utils;

namespace iAFWebHost.Models
{
    public class InboxConfigModel
    {
        [Required]
        public string InboxEmail { get; set; }
        public string InboxId { get { return InboxEmail.MD5();  } }
    }
}