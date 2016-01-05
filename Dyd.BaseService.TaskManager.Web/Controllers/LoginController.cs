using Dyd.BaseService.TaskManager.Domain.Dal;
using Dyd.BaseService.TaskManager.Domain.Model;
using Dyd.BaseService.TaskManager.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using XXF.BasicService.CertCenter;

namespace Dyd.BaseService.TaskManager.Web.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        //登录
        [HttpGet]
        public ActionResult Login(string appid, string sign, string returnurl)
        {
            XXF.BasicService.CertCenter.CertCenterProvider ccp = new XXF.BasicService.CertCenter.CertCenterProvider(XXF.BasicService.CertCenter.ServiceCertType.manage);
            if (!string.IsNullOrEmpty(appid))
            {
                if (string.IsNullOrWhiteSpace(returnurl))
                {
                    throw new Exception("returnurl错误！");
                }
                string appsecret = ccp.GetAppSecret(appid);
                if (appsecret == "")
                {
                    throw new Exception("appid不存在！");
                }
                Dictionary<string, string> para = new Dictionary<string, string>();//需要参加签名的参数对
                para.Add("appid", appid);
                para.Add("returnurl", returnurl);
                if (sign != Common.GetSign(para, appsecret))
                {
                    throw new Exception("签名错误！");
                }
                if (User.Identity.IsAuthenticated)//已登录过
                {
                    string[] tokens = User.Identity.Name.Split(',');
                    if (tokens.Count() > 1)
                    {
                        return RedirectToAction("index", "Task");
                    }
                }
            }
            else
            {

            }
            return View();
        }

        //登录
        [HttpPost]
        public ActionResult Login(string appid, string sign, string returnurl, string username, string password, string validate)
        {

            if (System.Configuration.ConfigurationManager.AppSettings["loginType"] == "1")
            {
                tb_user_model user = Common.GetUser(username, password);
                if (null != user)
                {                   
                    if (user == null)
                        throw new Exception("用户在平台中未开权限。");
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(user.id + " " + user.username + " " + username + "," + "token" + " " + user.userrole, false, (int)FormsAuthentication.Timeout.TotalMinutes);
                    string enticket = FormsAuthentication.Encrypt(ticket);
                    HttpCookie cookieofau = new HttpCookie(FormsAuthentication.FormsCookieName, enticket);
                    Response.AppendCookie(cookieofau);
                    return RedirectToAction("index", "Task", new { userid = user.id });
                }
            }
            try
            {
                returnurl = returnurl ?? "";
                username = username ?? "";
                password = password ?? "";
                validate = validate ?? "";
                ViewBag.username = username;
                XXF.BasicService.CertCenter.CertCenterProvider ccp = new XXF.BasicService.CertCenter.CertCenterProvider(XXF.BasicService.CertCenter.ServiceCertType.manage);
                if (!string.IsNullOrEmpty(appid))
                {   //外部授权
                    if (returnurl.Length < 2)
                    {
                        throw new Exception("returnurl错误！");
                    }
                    string appsecret = ccp.GetAppSecret(appid);
                    if (appsecret == "")
                    {
                        throw new Exception("appid不存在！");
                    }
                    Dictionary<string, string> para = new Dictionary<string, string>();//需要参加签名的参数对
                    para.Add("appid", appid);
                    para.Add("returnurl", returnurl);
                    if (sign != Common.GetSign(para, appsecret))
                    {
                        throw new Exception("签名错误！");
                    }
                }
                AuthToken re = ccp.Login(username, password);
                if (re != null)
                {
                    username = re.userid;
                    #region 6写auth Cookie
                    tb_user_model user = Common.GetUserName(username);
                    if (user == null)
                        throw new Exception("用户在平台中未开权限。");
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(user.id + " " + user.username + " " + username + "," + re.token + " " + user.userrole, false, (int)FormsAuthentication.Timeout.TotalMinutes);
                    string enticket = FormsAuthentication.Encrypt(ticket);
                    HttpCookie cookieofau = new HttpCookie(FormsAuthentication.FormsCookieName, enticket);
                    Response.AppendCookie(cookieofau);
                    #endregion
                    return RedirectToAction("index", "Task", new { userid = user.id });
                }
                else
                {
                    ModelState.AddModelError("", ccp.result.msg);
                    return View();
                }
            }
            catch (Exception exp)
            {
                ModelState.AddModelError("", "登陆出错,请咨询管理员。错误信息:" + exp.Message);
                return View();
            }
        }

        //登出
        public ActionResult Logout(string returnurl)
        {
            if (User.Identity.IsAuthenticated)
            {
                XXF.BasicService.CertCenter.CertCenterProvider ccp = new XXF.BasicService.CertCenter.CertCenterProvider(XXF.BasicService.CertCenter.ServiceCertType.manage);
                HttpCookie authcookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authcookie.Value);
                string userid = ticket.Name.Split(' ').FirstOrDefault();
                ccp.LogOut(ticket.Name.Split(',').LastOrDefault());
                FormsAuthentication.SignOut();
            }
            if (string.IsNullOrEmpty(returnurl))
                return RedirectToAction("Login");
            else
                return Redirect(returnurl);
        }


    }
}
