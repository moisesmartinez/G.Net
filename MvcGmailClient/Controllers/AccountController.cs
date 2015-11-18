using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcGmailClient.Controllers
{
    public class AccountController : Controller
    {
        MyGmailDBEntities myEntities = new MyGmailDBEntities();

        [AllowAnonymous]
        public ActionResult Login()
        {
            //If an user is already authenticated, then redirect to main page.
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Models.Login mLogin)
        {
            try
            {
                //Look for the username+password
                MvcGmailClient.User user = myEntities.Users.Where(userLinq => userLinq.UserName.Equals(mLogin.Username) && userLinq.Password.Equals(mLogin.Password)).FirstOrDefault();
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.UserName, mLogin.RememberMe);
                    Session["UserSession"] = user.UserId;

                    string returnUrl = Request["ReturnUrl"];
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewData["Error"] = " Username/Password are not correct!";
                }
            }
            catch (Exception ex)
            {

            }
            ModelState.Remove("Password");
            return View();
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            //If an user is already authenticated, then redirect to main page.
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public JsonResult doesUsernameExists(string Username)
        {
            MvcGmailClient.User user = myEntities.Users.Where(userLinq => userLinq.UserName.Equals(Username)).FirstOrDefault();
            //If didn't found the Username, return true
            if (user == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //if the Username is found, return error message
                return Json(string.Format("Username '{0}' is already taken", Username), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Models.Register mRegister)
        {
            //If an user is already authenticated, then redirect to main page.
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                //Check if the new username is already 
                MvcGmailClient.User user = myEntities.Users.Where(userLinq => userLinq.UserName.Equals(mRegister.Username)).FirstOrDefault();
                if (user == null) //if I didn't find the username, then create the new User
                {
                    MvcGmailClient.User newUser = new User();
                    newUser.UserName = mRegister.Username;
                    newUser.Password = mRegister.Password;
                    myEntities.Users.Add(newUser);
                    myEntities.SaveChanges();
                    FormsAuthentication.SetAuthCookie(newUser.UserName, false);
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {

            }

            //If I was not previously redirected (hence, failed to create user), render normal view.
            ModelState.Remove("Password");
            ModelState.Remove("PasswordConfirmation");
            ModelState.Remove("AgreeWithTerms");
            return View();
        }


        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }



    }
}
