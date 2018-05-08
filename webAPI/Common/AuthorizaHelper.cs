using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Security;

namespace webAPI.Common
{
    public static class AuthorizaHelper
    {
        private static string msg = string.Empty;
        public static string InitializeTicket(string custNo,string password)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(0, custNo, DateTime.Now, DateTime.Now.AddHours(1), false, string.Format("{0}&{1}", custNo, password));
            var ticketString = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(ticketString, ticketString);
            cookie.HttpOnly = true;
            if(ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            HttpContext context = HttpContext.Current;
            if (context == null)
                throw new InvalidOperationException();
            context.Response.Cookies.Remove(cookie.Name);
            context.Response.Cookies.Add(cookie);
            var identity = new GenericIdentity(custNo);
            identity.AddClaim(new System.Security.Claims.Claim(GenericIdentity.DefaultNameClaimType, custNo));
            SetPrincipal(new GenericPrincipal(identity, null));
            msg = string.Format("[User Ticket]:CNO:{0};T:{1};", custNo, ticketString);
            Log.i(msg);
            return ticketString;
        }

        private static void SetPrincipal(GenericPrincipal principal)
        {
            //throw new NotImplementedException();
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
                HttpContext.Current.User = principal;
        }
    }
}