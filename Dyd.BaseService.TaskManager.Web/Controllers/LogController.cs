using Dyd.BaseService.TaskManager.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XXF.Db;
using Dyd.BaseService.TaskManager.Domain.Model;
using Dyd.BaseService.TaskManager.Domain.Dal;
using Webdiyer.WebControls.Mvc;

namespace Dyd.BaseService.TaskManager.Web.Controllers
{
    [AuthorityCheck]
    public class LogController : BaseWebController
    {
        //
        // GET: /Log/
        public ActionResult ErrorLog(string keyword, string CStime, string CEtime, int id = -1, int errortype = -1, int taskid = -1, int nodeid = -1, int pagesize = 10, int pageindex = 1)
        {
            return this.Visit(Core.EnumUserRole.None, () =>
            {
                int count = 0;
                using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                {
                    ViewBag.keyword = keyword; ViewBag.CStime = CStime; ViewBag.CEtime = CEtime; ViewBag.id = id; ViewBag.errortype = errortype; ViewBag.taskid = taskid;
                    ViewBag.nodeid = nodeid; ViewBag.pagesize = pagesize; ViewBag.pageindex = pageindex;
                    PubConn.Open();
                    tb_error_dal dal = new tb_error_dal();
                    List<tb_errorinfo_model> model = dal.GetList(PubConn, keyword, id, CStime, CEtime, errortype, taskid, nodeid, pagesize, pageindex, out count);
                    PagedList<tb_errorinfo_model> pageList = new PagedList<tb_errorinfo_model>(model, pageindex, pagesize, count);
                    List<tb_task_model> Task = new tb_task_dal().GetListAll(PubConn);
                    List<tb_node_model> Node = new tb_node_dal().GetListAll(PubConn);
                    ViewBag.Node = Node;
                    ViewBag.Task = Task;
                    return View(pageList);
                }
            });
        }

        public ActionResult Log(string keyword, string CStime, string CEtime, int id = -1, int logtype = -1, int taskid = -1, int nodeid = -1, int pagesize = 10, int pageindex = 1)
        {
            return this.Visit(Core.EnumUserRole.None, () =>
            {
                ViewBag.keyword = keyword; ViewBag.CStime = CStime; ViewBag.CEtime = CEtime; ViewBag.id = id; ViewBag.logtype = logtype; ViewBag.taskid = taskid;
                ViewBag.nodeid = nodeid; ViewBag.pagesize = pagesize; ViewBag.pageindex = pageindex;
                int count = 0;
                using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                {
                    PubConn.Open();
                    tb_log_dal dal = new tb_log_dal();
                    List<tb_loginfo_model> model = dal.GetList(PubConn, keyword, id, CStime, CEtime, logtype, taskid, nodeid, pagesize, pageindex, out count);
                    PagedList<tb_loginfo_model> pageList = new PagedList<tb_loginfo_model>(model, pageindex, pagesize, count);
                    List<tb_task_model> Task = new tb_task_dal().GetListAll(PubConn);
                    List<tb_node_model> Node = new tb_node_dal().GetListAll(PubConn);
                    ViewBag.Node = Node;
                    ViewBag.Task = Task;
                    return View(pageList);
                }
            });
        }
    }
}