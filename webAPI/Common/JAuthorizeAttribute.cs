using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Security;

namespace webAPI.Common
{
    /// <summary>
    /// 获取
    /// </summary>
    public class JAuthorizeAttribute:AuthorizeAttribute
    {
        private string msg = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //base.OnAuthorization(actionContext);
            var authorization = actionContext.Request.Headers.Authorization;
            msg = string.Format("[JAuthorization]Action:{0},Authorization:{1}", actionContext.Request.RequestUri.AbsolutePath, authorization);
            Log.i(msg);
            if(authorization!=null && authorization.Parameter!=null)
            {
                var encryptTicket = authorization.Parameter;
                if(ValidateTicket(encryptTicket))
                {
                    base.IsAuthorized(actionContext);
                }
                else
                {
                    HandleUnauthorizedRequest(actionContext);
                }
            }
            else
            {
                var attributes = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
                bool isAnonymous = attributes.Any(a => a is AllowAnonymousAttribute);
                if (isAnonymous) base.OnAuthorization(actionContext);
                else HandleUnauthorizedRequest(actionContext);
            }
        }

        private bool ValidateTicket(string encryptTicket)
        {
            //throw new NotImplementedException();
            try
            {
                var ticket = FormsAuthentication.Decrypt(encryptTicket);
                if(!ticket.IsPersistent && ticket.Expiration<DateTime.Now)
                {
                    Log.i("身份验证过期");
                    return false;
                }
                var strTicket = ticket.UserData;
                msg = string.Format("[Ticket String]:{0};", strTicket);
                var index = strTicket.IndexOf("&");
                string custno = strTicket.Substring(0, index);
                string custpwd = strTicket.Substring(index + 1);
                return true;
            }
            catch(Exception ex)
            {
                Log.e("ticket错误",ex);                
            }
            return false;
        }
    }
}