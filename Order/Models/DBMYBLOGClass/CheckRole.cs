using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Order.Models.DBMYBLOGClass
{
    public class CheckRole
    {
        public void SetRole(string userName, string roleName)
        {
            SetAuthCookie(userName, roleName);
        }
        private void SetAuthCookie(string userName, string roleName)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddMinutes(10), false, roleName);
            string encryptTicket = FormsAuthentication.Encrypt(ticket);
            System.Web.HttpCookie authCookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, encryptTicket);
            System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);
        }
    }
}