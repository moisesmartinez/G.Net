using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MvcGmailClient.Models
{
    public class Register
    {
        [Required(ErrorMessage = "Username Required", AllowEmptyStrings = false)]
        public string Username { get; set; }

        //[Required(ErrorMessage = "Email Required", AllowEmptyStrings = false)]
        //public string Email { get; set; }

        [Required(ErrorMessage = "Password Required", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password Required", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string Password2 { get; set; }

        public bool AgreeWithTerms { get; set; }
    }
}