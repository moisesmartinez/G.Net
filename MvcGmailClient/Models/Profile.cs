using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MvcGmailClient.Models
{
    public class Profile
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPassword2 { get; set; }
        
        public string GmailAddress { get; set; }
        public string GmailPassword { get; set; }
    }
}