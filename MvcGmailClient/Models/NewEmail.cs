using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcGmailClient.Models
{
    public class NewEmail
    {
        public string EmailTo { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}