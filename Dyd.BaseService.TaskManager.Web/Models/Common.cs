using Dyd.BaseService.TaskManager.Domain.Dal;
using Dyd.BaseService.TaskManager.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XXF.Db;

namespace Dyd.BaseService.TaskManager.Web.Models
{
    public class Common
    {
        private static object k = new object() { };
        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="paralist">需要进行签名的参数</param>
        /// <param name="appsecret">app密钥</param>
        /// <returns></returns>
        public static string GetSign(Dictionary<string, string> paralist, string appsecret)
        {
            if (paralist == null)
            {
                return null;
            }

            List<string> para = new List<string>();
            foreach (var a in paralist)
            {
                para.Add(a.Key + a.Value);
            }
            para.Sort();
            string centerstr = appsecret + string.Concat(para) + appsecret;

            return LibCrypto.MD5(centerstr);
        }

        public static tb_user_model GetUserName(string userstaffno)
        {
            using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
            {
                PubConn.Open();
                tb_user_dal dal = new tb_user_dal();
                return dal.GetUserName(PubConn, userstaffno);
            }
        }

        public static tb_user_model GetUser(string userstaffno, string userpsw)
        {
            using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
            {
                PubConn.Open();
                tb_user_dal dal = new tb_user_dal();
                return dal.GetUser(PubConn, userstaffno, userpsw);
            }
        }


        public static string HelpHtml(string helpinfo = "使用说明")
        {
            return "<img src='/content/images/help.png' style='' width='20' height='20' title='" + helpinfo + "'></img>";
        }

        public static int GetUserId(Controller con)
        {
            string user = con.User.Identity.Name.Split(',')[0];
            int id = Convert.ToInt32(user.Split(' ').LastOrDefault());
            return id;
        }

        public static int GetAvailableNode()
        {
            lock (k)
            {
                using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                {
                    PubConn.Open();
                    int id = new tb_node_dal().GetAvailableNode(PubConn);
                    return id;
                }
            }
        }
    }

    public class PostChangeModel
    {
        public int id { get; set; }
        public int nodeid { get; set; }
        public int state { get; set; }
    }
}