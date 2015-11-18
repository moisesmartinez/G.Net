using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcGmailClient.Models
{
    public class Email
    {
        public int Uid { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
    }
}