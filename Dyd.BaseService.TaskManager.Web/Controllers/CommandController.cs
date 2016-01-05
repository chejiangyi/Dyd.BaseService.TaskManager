using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dyd.BaseService.TaskManager.Domain.Model;
using Dyd.BaseService.TaskManager.Domain.Dal;
using XXF.Db;
using Dyd.BaseService.TaskManager.Web.Models;
using Webdiyer.WebControls.Mvc;

namespace Dyd.BaseService.TaskManager.Web.Controllers
{
    [AuthorityCheck]
    public class CommandController : BaseWebController
    {
        //
        // GET: /Command/

        public ActionResult Index(int taskid = -1, int commandstate = -1, int nodeid = -1, int pagesize = 10, int pageindex = 1)
        {
            return this.Visit(Core.EnumUserRole.Admin, () =>
            {
                ViewBag.taskid = taskid; ViewBag.commandstate = commandstate; ViewBag.nodeid = nodeid; ViewBag.pagesize = pagesize; ViewBag.pageindex = pageindex;
                tb_command_dal dal = new tb_command_dal();
                PagedList<tb_command_model_Ex> pageList = null;
                int count = 0;
                using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                {
                    PubConn.Open();
                    List<tb_command_model_Ex> List = dal.GetList(PubConn, commandstate, taskid, nodeid, pagesize, pageindex, out count);
                    List<tb_task_model> Task = new tb_task_dal().GetListAll(PubConn);
                    List<tb_node_model> Node = new tb_node_dal().GetListAll(PubConn);
                    ViewBag.Node = Node;
                    ViewBag.Task = Task;
                    pageList = new PagedList<tb_command_model_Ex>(List, pageindex, pagesize, count);
                }
                return View(pageList);
            });
        }

        public ActionResult Add()
        {
            return this.Visit(Core.EnumUserRole.Admin, () =>
            {
                using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                {
                    PubConn.Open();
                    List<tb_task_model> Task = new tb_task_dal().GetListAll(PubConn);
                    List<tb_node_model> Node = new tb_node_dal().GetListAll(PubConn);
                    ViewBag.Node = Node;
                    ViewBag.Task = Task;
                    return View();
                }
            });
        }

        [HttpPost]
        public ActionResult Add(tb_command_model model)
        {
            return this.Visit(Core.EnumUserRole.Admin, () =>
            {
                tb_command_dal dal = new tb_command_dal();
                using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                {
                    PubConn.Open();
                    model.commandcreatetime = DateTime.Now;
                    dal.Add(PubConn, model);
                }
                return RedirectToAction("index");
            });
        }

        public ActionResult Update(int id)
        {
            return this.Visit(Core.EnumUserRole.Admin, () =>
            {
                using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                {
                    PubConn.Open();
                    tb_command_dal dal = new tb_command_dal();
                    tb_command_model_Ex model = dal.GetOneCommand(PubConn, id);
                    List<tb_node_model> Node = new tb_node_dal().GetListAll(PubConn);
                    ViewBag.Node = Node;
                    return View(model);
                }
            });
        }

        [HttpPost]
        public ActionResult Update(tb_command_model model)
        {
            return this.Visit(Core.EnumUserRole.Admin, () =>
            {
                tb_command_dal dal = new tb_command_dal();
                using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                {
                    PubConn.Open();
                    model.commandcreatetime = DateTime.Now;
                    dal.UpdateCommand(PubConn, model);
                }
                return RedirectToAction("index");
            });
        }

        public JsonResult Delete(int id)
        {
            return this.Visit(Core.EnumUserRole.Admin, () =>
            {
                using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                {
                    PubConn.Open();
                    tb_command_dal dal = new tb_command_dal();
                    dal.Delete(PubConn, id);
                    return Json(new { code = 1, msg = "Success" });
                }
            });
        }
    }
}
