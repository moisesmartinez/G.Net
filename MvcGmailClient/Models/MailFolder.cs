using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcGmailClient.Models
{
    public class MailFolder
    {
        public NewEmail ComposeEmail { get; set; }
        public List<Email> Emails { get; set; }

        public MailFolder()
        {
            ComposeEmail = new NewEmail();
            Emails = new List<Email>();
        }

    }
}