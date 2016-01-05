using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dyd.BaseService.TaskManager.Domain.Dal;
using Dyd.BaseService.TaskManager.Domain.Model;
using Dyd.BaseService.TaskManager.Web.Models;
using Webdiyer.WebControls.Mvc;
using XXF.Db;

namespace Dyd.BaseService.TaskManager.Web.Controllers
{
    public class PerformanceController : BaseWebController
    {
        //
        // GET: /Performance/

        public ActionResult Index(string nodeid, string taskid, string orderby)
        {
            return this.Visit(Core.EnumUserRole.None, () =>
            {
                ViewBag.nodeid = nodeid;
                ViewBag.taskid = taskid;
                ViewBag.orderby = orderby;
                tb_performance_dal dal = new tb_performance_dal();
                
                using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                {
                    PubConn.Open();
                    ViewBag.taskmodels = dal.GetAllWithTask(PubConn,nodeid,taskid,orderby,DateTime.Now.AddMinutes(-10));
                   
                }
                return View();
            });
        }
        public ActionResult NodeIndex()
        {
            return this.Visit(Core.EnumUserRole.None, () =>
            {
                tb_performance_dal dal = new tb_performance_dal();
                using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                {
                    ViewBag.nodemodels = dal.GetAllWithNode(PubConn, "p.nodeid desc", DateTime.Now.AddMinutes(-10));
                }
                return View();
            });
        }
    }
}
