using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
//Library for counting Gmail messages
using AE.Net.Mail;

namespace MvcGmailClient.Controllers
{
    public class HomeController : Controller
    {
        MyGmailDBEntities myEntities = new MyGmailDBEntities();

        [Authorize]
        public ActionResult Index()
        {
            if (Session["UserSession"] != null)
            {
                try
                {
                    //Get the UserId and then retrieve his email credentials.
                    int userId = Convert.ToInt32(Session["UserSession"]);
                    MvcGmailClient.User user = myEntities.Users.Where(userLinq => userLinq.UserId == (userId)).FirstOrDefault();
                    ViewData["Username"] = user.UserName;

                    if (user.Gmails.Count > 0)
                    {
                        //Finally, with the email credentials, search how many unread mails the User has.
                        var imap = new ImapClient("imap.gmail.com", user.Gmails.ElementAt(0).GmailAddress, user.Gmails.ElementAt(0).GmailPassword, AuthMethods.Login, 993, true);
                        ViewData["NewMessagesCount"] = imap.SearchMessages(SearchCondition.Unseen(), true).Count();
                        ViewData["GmailAddress"] = true;
                        imap.Logout();
                        imap.Disconnect();
                    }
                    else
                    {
                        ViewData["Error"] = "No Gmail account found.";
                    }
                }
                catch (Exception ex)
                {
                    ViewData["Error"] = "Gmail credentials failed...";
                }
            }
            return View();
        }

        [Authorize]
        public ActionResult Profile()
        {
            //Get an user goes to his profile page, show what Gmail the user has.
            Models.Profile mProfile = new Models.Profile();
            try
            {
                int userId = Convert.ToInt32(Session["UserSession"]);
                MvcGmailClient.Gmail gmailAccount = myEntities.Gmails.Where(gmailLinq => gmailLinq.UserId == (userId)).FirstOrDefault();

                //If I found an Gmail, send it to the View through a model
                if (gmailAccount != null)
                {
                    mProfile.GmailAddress = gmailAccount.GmailAddress;
                }
            }
            catch (Exception ex)
            {

            }
            return View(mProfile);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Profile(Models.Profile mProfile, string SubmitButton)
        {
            try
            {
                //Local variables
                string oldPassword = mProfile.OldPassword;
                string newPassword = mProfile.NewPassword;
                string gmailAddress = mProfile.GmailAddress;
                string gmailPassword = mProfile.GmailPassword;
                int userId = Convert.ToInt32(Session["UserSession"]);
                
                //There are 3 submit buttons in the Profile Page, so I have conditional switch for the SubmitButton
                switch (SubmitButton)
                {
                    //Profile Button = update account password
                    case "Profile":
                        //Find the user in the database 
                        MvcGmailClient.User user = myEntities.Users.Where(userLinq => userLinq.UserId == (userId)).FirstOrDefault();
                        // if the password in the DB == submitted old password
                        if (user.Password == oldPassword)
                        {
                            //Change the password and try save in the DB
                            user.Password = newPassword;
                            try 
                            { 
                                myEntities.SaveChanges(); 
                                TempData["success"] = "Password updated successfully!"; 
                            }
                            catch 
                            { 
                                TempData["failed"] = "Couldn't save the new password!"; 
                            }
                        }
                        else
                        {
                            TempData["failed"] = "Couldn't save the new password!";
                        }
                        break;

                    //Gmail button = Updating the user's Gmail Address
                    case "Gmail":
                        //TO-DO: Be able to add multiple gmail accounts.
                        //Fin an existing Gmail
                        MvcGmailClient.Gmail newGmail = myEntities.Gmails.Where(gmailLinq => gmailLinq.UserId == (userId)).FirstOrDefault();
                        bool isNewGmail = false;
                        if (newGmail == null)//If I couldn't find an existing Gmail, create one
                        {
                            newGmail = new Gmail();
                            isNewGmail = true;
                        }
                        newGmail.GmailAddress = gmailAddress;
                        newGmail.GmailPassword = gmailPassword;
                        newGmail.UserId = userId;
                        if (isNewGmail)
                            myEntities.Gmails.Add(newGmail);
                        //try to save the new Gmail in the DB
                        try 
                        { 
                            myEntities.SaveChanges(); 
                            TempData["success"] = "Mail Credentials updated successfully!"; 
                        }
                        catch 
                        {
                            TempData["failed"] = "Couldn't update the new Gmail!";
                        }

                        break;

                    //RemoveGmail button = The user wants the delete the Gmail Address from the DB
                    case "RemoveGmail":
                        MvcGmailClient.Gmail removeGmail = myEntities.Gmails.Where(gmailLinq => gmailLinq.UserId == (userId)).FirstOrDefault();
                        //if the user clicked this button and has a Gmail Address, delete it
                        if (removeGmail != null)
                        {
                            myEntities.Gmails.Remove(removeGmail);
                            //try to save the Gmail information in the DB
                            try
                            {
                                myEntities.SaveChanges();
                                TempData["success"] = "Gmail removed successfully!";
                            }
                            catch
                            {
                                TempData["failed"] = "Couldn't remove the actual Gmail!";
                            }
                        }
                        else
                        {
                            //if the user clicked this button but does not have a Gmail Address, show this error message
                            TempData["failed"] = "You don't have a Gmail associated to your account!";
                        }
                        break;
                }
            }
            catch (Exception ex)
            {

            }
            //I use the redirect to action in other to load properly both form in the View (otherwise, the view will only render the submitted form)
            return RedirectToAction("Profile", "Home");
        }

    }
}
