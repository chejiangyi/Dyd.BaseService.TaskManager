using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dyd.BaseService.TaskManager.Core;

namespace Dyd.BaseService.TaskManager.Web.Models
{
    public class BaseWebController : Controller
    {
        /// <summary>
        /// web 访问控制器
        /// 错误拦截
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public ActionResult Visit(EnumUserRole role,Func<ActionResult> action)
        {
            return this.Visit<ActionResult>(role, action);
        }

        /// <summary>
        /// web 访问控制器
        /// 错误拦截
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public T Visit<T>(EnumUserRole userrole, Func<T> action)
        {
            try
            {
                string role = UserRole;
                string Number = UserNumber;
                if ((int)userrole == Convert.ToInt32(role) || (int)userrole == -1)
                {
                    ViewBag.Role = role;
                    ViewBag.Number = Number;
                    return action.Invoke();
                }
                else
                {
                    throw new Exception("无权访问！");
                }
            }
            catch (Exception exp)
            {
                //异常返回
                throw exp;
            }
        }

       

        public string UserRole
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                {
                    string name = User.Identity.Name;
                    if (name != null)
                    {
                        var role = name.Split(' ').LastOrDefault();
                        return role;
                    }
                }
                return "norole";
            }
        }

        public string UserNumber
        {
            get 
            {
                if (User.Identity.IsAuthenticated)
                {
                    string name = User.Identity.Name;
                    if (name != null)
                    {
                        var Number = name.Split(' ').FirstOrDefault();
                        return Number;
                    }
                }
                return "";
            }
        }


    }
}