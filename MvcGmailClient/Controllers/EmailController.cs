using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//Libraries for sending Gmail messages
using System.Net;
using System.Net.Mail;
//Library for recieving Gmail messages
using AE.Net.Mail;

namespace MvcGmailClient.Controllers
{
    public class EmailController : Controller
    {
        MyGmailDBEntities myEntities = new MyGmailDBEntities();

        public string GetMailboxName(string folderName)
        {
            string mailboxName = "";
            switch (folderName)
            {
                case "Inbox":
                    mailboxName = "INBOX";
                    break;
                case "Sent":
                    mailboxName = "[Gmail]/Enviados"; //if your Gmail region is USA (or english speaking), then use "[Gmail]/Sent"
                    break;
                case "Trash":
                    mailboxName = "[Gmail]/Eliminados";//if your Gmail region is USA (or english speaking), then use "[Gamil]/Trash"
                    break;
            }
            return mailboxName;
        }

        //This function will try to return at much 10 email messages.
        public Models.MailFolder GetFolderMessages(string folderName, int pageNumber)
        {
            Models.MailFolder folderMessages = new Models.MailFolder();

            //Load how many unread emails are in each folder
            if (Session["UserSession"] != null)
            {
                int userId = Convert.ToInt32(Session["UserSession"]);
                MvcGmailClient.User user = myEntities.Users.Where(userLinq => userLinq.UserId == (userId)).FirstOrDefault();
                ViewData["Username"] = user.UserName;

                if (user.Gmails.Count > 0)
                {
                    try
                    {
                        string mailboxName = GetMailboxName(folderName);
                        
                        var imapClient = new ImapClient("imap.gmail.com", user.Gmails.ElementAt(0).GmailAddress, user.Gmails.ElementAt(0).GmailPassword, AuthMethods.Login, 993, true);
                        imapClient.SelectMailbox(mailboxName);
                        //Find the messages range. If you have 100 messages, and I want to show the last 10 messages, I'll have to get and return the messages 100, 99, ..., 91 (100-91)
                        int totalMessagesCount = imapClient.GetMessageCount();
                        int minMsg = totalMessagesCount - (pageNumber * 10);
                        int maxMsg = minMsg + 10 - 1;
                        //After calculating the messages range, use it and get the email messages.
                        var msgs = imapClient.GetMessages(minMsg, maxMsg, true);
                        
                        //Parse the AE.NET.Mail.Messages and create my model
                        int msgsCount = msgs.Count();
                        for (int i = msgsCount - 1; i >= 0; i--)
                        {
                            //Get AE.NET.Mail.Message
                            AE.Net.Mail.MailMessage mailMessage = msgs[i];

                            //Create and set model
                            Models.Email mail = new Models.Email();
                            mail.Subject = mailMessage.Subject;
                            mail.Date = mailMessage.Date;
                            if (mailMessage.From.DisplayName == "")
                            {
                                mail.From = mailMessage.From.Address;
                            }
                            else
                            {
                                mail.From = mailMessage.From.DisplayName;
                            }

                            if (mail.Body == null) mail.Body = "";
                            if (mail.Body.Length > 40)
                            {
                                mail.Body = mail.Body.Substring(0, 40) + "...";
                            }
                            int uid = 0;
                            int.TryParse(mailMessage.Uid, out uid);
                            mail.Uid = uid;

                            //Add my Models.Email to my main model (Models.MailFolder)
                            folderMessages.Emails.Add(mail);
                        }

                        //Finally, set a few ViewData parameters (Displayed rage, How many messages and how many unread messages are in the Inbox mailbox)
                        ViewData["MessageRange"] = string.Format("{0}-{1}", totalMessagesCount - maxMsg, totalMessagesCount - minMsg);
                        ViewData["MessageCount"] = totalMessagesCount;
                        imapClient.SelectMailbox("INBOX");
                        ViewData["NewMessagesCount"] = imapClient.SearchMessages(SearchCondition.Unseen(), true).Count();

                        //Close connections
                        imapClient.Logout();
                        imapClient.Disconnect();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return folderMessages;
        }

        public bool isValidRequestPage()
        {
            //If I couldn't find a "page" parameter in my request parameters, return false
            if (Request["page"] == null)
                return false;

            int pageNumber = -1;
            //if "page" parameter is not a number, return false
            if (!int.TryParse(Request["page"], out pageNumber))
                return false;

            //if "page" parameter is less than zero
            if (pageNumber < 0)
                return false;

            return true;
        }

        public ActionResult Inbox()
        {
            if (!isValidRequestPage())
            {
                return RedirectToAction("Inbox", "Email", new { page = "1" });
            }
            ViewData["InboxActive"] = "active";
            Models.MailFolder mFolder = GetFolderMessages("Inbox", Convert.ToInt32(Request["page"]));
            return View(mFolder);
        }

        public ActionResult Sent()
        {
            if (!isValidRequestPage())
            {
                return RedirectToAction("Sent", "Email", new { page = "1" });
            }
            ViewData["SentActive"] = "active";
            Models.MailFolder mFolder = GetFolderMessages("Sent", Convert.ToInt32(Request["page"]));
            return View(mFolder);
        }

        public ActionResult Trash()
        {
            if (!isValidRequestPage())
            {
                return RedirectToAction("Trash", "Email", new { page = "1" });
            }
            ViewData["TrashActive"] = "active";
            Models.MailFolder mFolder = GetFolderMessages("Trash", Convert.ToInt32(Request["page"]));
            return View(mFolder);
        }

        [HttpPost]
        public JsonResult SendEmail(Models.MailFolder newEmail)
        {
            //
            var fromAddress = new System.Net.Mail.MailAddress("moy.contrabass@gmail.com", "From Name");
            var toAddress = new System.Net.Mail.MailAddress(newEmail.ComposeEmail.EmailTo);
            const string fromPassword = "asdas";
            string subject = newEmail.ComposeEmail.Subject;
            string body = newEmail.ComposeEmail.Message;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new System.Net.Mail.MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }            
            return Json(true);
        }

        [HttpPost]
        public JsonResult GetMail(int Uid, string folderName)
        {
            //Get the user session and retrieve his Gmail Credentials
            if (Session["UserSession"] != null)
            {
                int userId = Convert.ToInt32(Session["UserSession"]);
                MvcGmailClient.User user = myEntities.Users.Where(userLinq => userLinq.UserId == (userId)).FirstOrDefault();
                ViewData["Username"] = user.UserName;

                if (user.Gmails.Count > 0)
                {
                    try
                    {
                        string mailboxName = GetMailboxName(folderName);

                        //Create new imap instance..
                        var imapClient = new ImapClient("imap.gmail.com", user.Gmails.ElementAt(0).GmailAddress, user.Gmails.ElementAt(0).GmailPassword, AuthMethods.Login, 993, true);
                        //.. and set the mailbox
                        imapClient.SelectMailbox(mailboxName);

                        //Then, use the Uid from the parameters to get the Mail Message
                        AE.Net.Mail.MailMessage message = imapClient.GetMessage(Uid.ToString());

                        //Finally, create and set my model "Email"
                        Models.Email result = new Models.Email() { Body = message.Body, Date = message.Date, Subject = message.Subject, From = message.From.Address };
                        //If the mail has any Html body, set it in my model.
                        foreach (var bodyga in message.AlternateViews)
                        {
                            if (bodyga.ContentType == "text/html")
                            {
                                result.Body = bodyga.Body;
                            }
                        }
                        //Close connections and return Json
                        imapClient.Logout();
                        imapClient.Disconnect();
                        return Json(result);
                    }
                    catch (Exception ex)
                    {
                        //if something failed, send an error message.
                        return Json(new { Error = "<strong>Error!</strong> Couldn't find the requested Mail!" });
                    }
                }
            }
            return Json(true);
        }
    }
}
