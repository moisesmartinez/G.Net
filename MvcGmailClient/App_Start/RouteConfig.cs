using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcGmailClient
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            //routes.MapRoute("Default", "", new { controller = "Account", action = "Login" });
            //routes.MapRoute(name: "Home", url: "{controller}/{action}/", );
            
            //Main page
            routes.MapRoute(
                name: "Default",
                url: "",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            //Account's Routes
            routes.MapRoute(
                name: "Account/Login",
                url: "Login",
                defaults: new { controller = "Account", action = "Login"}
            );
            routes.MapRoute(
                name: "Account/Logout",
                url: "Logout",
                defaults: new { controller = "Account", action = "Logout" }
            );
            routes.MapRoute(
                name: "Account/Register",
                url: "Register",
                defaults: new { controller = "Account", action = "Register" }
            );
            //Helper route. Used in an ajax call in the Register page. 
            routes.MapRoute(
                name: "Account/doesUsernameExists",
                url: "doesUsernameExists",
                defaults: new { controller = "Account", action = "doesUsernameExists" }
            );

            //Home's Routes
            routes.MapRoute(
                name: "Home/Profile",
                url: "Profile",
                defaults: new { controller = "Home", action = "Profile" }
            );
            //routes.MapRoute(
            //    name: "Home/Inbox",
            //    url: "Inbox",
            //    defaults: new { controller = "Home", action = "Inbox" }
            //);

            //Email's Routes
            routes.MapRoute(
                name: "Email/Inbox",
                url: "Inbox",
                defaults: new { controller = "Email", action = "Inbox" }
            );
            routes.MapRoute(
                name: "Email/Sent",
                url: "Sent",
                defaults: new { controller = "Email", action = "Sent" }
            );
            routes.MapRoute(
                name: "Email/Trash",
                url: "Trash",
                defaults: new { controller = "Email", action = "Trash" }
            );
            routes.MapRoute(
                name: "Email/SendEmail",
                url: "Email/SendEmail",
                defaults: new { controller = "Email", action = "SendEmail" }
            );
            routes.MapRoute(
                name: "Email/GetMail",
                url: "Email/GetMail",
                defaults: new { controller = "Email", action = "GetMail" }
            );
        }
    }
}