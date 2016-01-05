using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dyd.BaseService.TaskManager.Web.Models
{
    public class Config
    {
        public static string TaskConnectString = System.Configuration.ConfigurationManager.AppSettings["TaskConnectString"].ToString();
    }
}