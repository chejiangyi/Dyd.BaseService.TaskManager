using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Dyd.BaseService.TaskManager.Web.Models
{
    public class AuthorityCheck : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                string url = httpContext.Request.Path;
                url = url.Substring(0, url.IndexOf("?") > 1 ? url.IndexOf("?") : url.Length);
                HttpCookie authcookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authcookie == null)
                {
                    string token = httpContext.Request.Form["token"].ToString();
                    XXF.BasicService.CertCenter.CertCenterProvider ccp = new XXF.BasicService.CertCenter.CertCenterProvider(XXF.BasicService.CertCenter.ServiceCertType.manage);
                    if (ccp.Auth(token))
                    {
                        return true;
                    }
                    return false;
                }
                try
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authcookie.Value);
                    string userid = ticket.Name.Split(' ').FirstOrDefault();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}