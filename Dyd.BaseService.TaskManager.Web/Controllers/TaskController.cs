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
using System.Drawing;
using System.IO;
using Dyd.BaseService.TaskManager.Core;
using XXF.Extensions;

namespace Dyd.BaseService.TaskManager.Web.Controllers
{
    [AuthorityCheck]
    public class TaskController : BaseWebController
    {
        //
        // GET: /Task/

        public ActionResult Index(string taskid, string keyword, string CStime,string CEtime, int categoryid = -1, int nodeid = -1, int userid = -1, int state = -999, int pagesize = 10, int pageindex = 1)
        {
            return this.Visit(Core.EnumUserRole.None, () =>
            {
                ViewBag.taskid = taskid;
                ViewBag.keyword = keyword;
                ViewBag.CStime = CStime;
                ViewBag.CEtime = CEtime;
                ViewBag.categoryid = categoryid;
                ViewBag.nodeid = nodeid;
                ViewBag.userid = userid;
                ViewBag.state = state;
                ViewBag.pagesize = pagesize;
                ViewBag.pageindex = pageindex;

                tb_task_dal dal = new tb_task_dal();
                PagedList<tb_tasklist_model> pageList = null;
                int count = 0;
                using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                {
                    PubConn.Open();
                    List<tb_tasklist_model> List = dal.GetList(PubConn,taskid, keyword, CStime,CEtime, categoryid, nodeid, userid, state, pagesize, pageindex, out count);
                    pageList = new PagedList<tb_tasklist_model>(List, pageindex, pagesize, count);
                    List<tb_node_model> Node = new tb_node_dal().GetListAll(PubConn);
                    List<tb_category_model> Category = new tb_category_dal().GetList(PubConn, "");
                    List<tb_user_model> User = new tb_user_dal().GetAllUsers(PubConn);
                    ViewBag.Node = Node;
                    ViewBag.Category = Category;
                    ViewBag.User = User;
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
                    List<tb_category_model> Category = new tb_category_dal().GetList(PubConn, "");
                    List<tb_node_model> Node = new tb_node_dal().GetListAll(PubConn);
                    List<tb_user_model> User = new tb_user_dal().GetAllUsers(PubConn);
                    ViewBag.Node = Node;
                    ViewBag.Category = Category;
                    ViewBag.User = User;
                    return View();
                }
            });
        }

        [HttpPost]
        public ActionResult Add(HttpPostedFileBase TaskDll, tb_task_model model, string tempdatajson)
        {
            return this.Visit(Core.EnumUserRole.Admin, () =>
            {
                string filename = TaskDll.FileName;
                Stream dll = TaskDll.InputStream;
                byte[] dllbyte = new byte[dll.Length];
                dll.Read(dllbyte, 0, Convert.ToInt32(dll.Length));
                tb_task_dal dal = new tb_task_dal();
                tb_version_dal dalversion = new tb_version_dal();
                tb_tempdata_dal tempdatadal = new tb_tempdata_dal();
                //model.taskcreateuserid = Common.GetUserId(this);
                using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                {
                    PubConn.Open();
                    model.taskcreatetime = DateTime.Now;
                    model.taskversion = 1;
                    int taskid = dal.AddTask(PubConn, model);
                    dalversion.Add(PubConn, new tb_version_model()
                    {
                        taskid = taskid,
                        version = 1,
                        versioncreatetime = DateTime.Now,
                        zipfile = dllbyte,
                        zipfilename = System.IO.Path.GetFileName(filename)
                    });
                    tempdatadal.Add(PubConn, new tb_tempdata_model()
                    {
                        taskid = taskid,
                        tempdatajson = tempdatajson,
                        tempdatalastupdatetime = DateTime.Now
                    });
                }
                return RedirectToAction("index");
            });
        }

        public ActionResult Update(int taskid)
        {
            return this.Visit(Core.EnumUserRole.None, () =>
            {
                using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                {
                    PubConn.Open();
                    tb_task_dal dal = new tb_task_dal();
                    tb_task_model model = dal.GetOneTask(PubConn, taskid);
                    tb_tempdata_model tempdatamodel = new tb_tempdata_dal().GetByTaskID(PubConn, taskid);
                    List<tb_version_model> Version = new tb_version_dal().GetTaskVersion(PubConn, taskid);
                    List<tb_category_model> Category = new tb_category_dal().GetList(PubConn, "");
                    List<tb_node_model> Node = new tb_node_dal().GetListAll(PubConn);
                    List<tb_user_model> User = new tb_user_dal().GetAllUsers(PubConn);
                    ViewBag.Node = Node;
                    ViewBag.Category = Category;
                    ViewBag.Version = Version;
                    ViewBag.User = User;
                    ViewBag.TempData = tempdatamodel;
                    return View(model);
                }
            });
        }

        [HttpPost]
        public ActionResult Update(HttpPostedFileBase TaskDll, tb_task_model model, string tempdatajson)
        {
            return this.Visit(Core.EnumUserRole.Admin, () =>
            {
                try
                {
                    tb_task_dal dal = new tb_task_dal();
                    tb_version_dal dalversion = new tb_version_dal();
                    tb_tempdata_dal tempdatadal = new tb_tempdata_dal();
                    byte[] dllbyte = null;
                    string filename = "";
                    int change = model.taskversion;
                    if (change == -1)
                    {
                        if (TaskDll == null)
                        {
                            throw new Exception("没有文件！");
                        }
                        filename = TaskDll.FileName;
                        Stream dll = TaskDll.InputStream;
                        dllbyte = new byte[dll.Length];
                        dll.Read(dllbyte, 0, Convert.ToInt32(dll.Length));
                        //model.taskcreateuserid = Common.GetUserId(this);
                    }
                    using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                    {
                        PubConn.Open();
                        var task = dal.GetOneTask(PubConn, model.id);
                        if (task.taskstate == (int)Dyd.BaseService.TaskManager.Core.EnumTaskState.Running)
                        {

                            throw new Exception("当前任务在运行中,请停止后提交");
                        }
                        if (change == -1)
                        {
                            model.taskversion = dalversion.GetVersion(PubConn, model.id) + 1;
                        }
                        model.taskupdatetime = DateTime.Now;
                        dal.UpdateTask(PubConn, model);
                        if (change == -1)
                        {
                            dalversion.Add(PubConn, new tb_version_model()
                            {
                                taskid = model.id,
                                version = model.taskversion,
                                versioncreatetime = DateTime.Now,
                                zipfile = dllbyte,
                                zipfilename = System.IO.Path.GetFileName(filename)
                            });
                        }
                        tempdatadal.UpdateByTaskID(PubConn, new tb_tempdata_model()
                        {
                            taskid = model.id,
                            tempdatajson = tempdatajson,
                            tempdatalastupdatetime = DateTime.Now
                        });
                        return RedirectToAction("index");
                    }
                }
                catch (Exception exp)
                {
                    ModelState.AddModelError("", exp.Message);
                    return View();
                }
            });
        }

        public JsonResult ChangeTaskState(int id, int nodeid, int state)
        {
            return this.Visit(Core.EnumUserRole.Admin, () =>
            {
                tb_command_dal dal = new tb_command_dal();
                tb_task_dal taskDal = new tb_task_dal();
                using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                {
                    PubConn.Open();
                    if (taskDal.CheckTaskState(PubConn, id) == state)
                    {
                        string msg = state == 1 ? "已开启" : "已关闭";
                        return Json(new { code = -1, msg = msg });
                    }
                    else
                    {
                        tb_command_model m = new tb_command_model()
                        {
                            command = "",
                            commandcreatetime = DateTime.Now,
                            commandname = state == (int)EnumTaskCommandName.StartTask ? EnumTaskCommandName.StartTask.ToString() : EnumTaskCommandName.StopTask.ToString(),
                            taskid = id,
                            nodeid = nodeid,
                            commandstate = (int)EnumTaskCommandState.None
                        };
                        dal.Add(PubConn, m);
                    }
                    return Json(new { code = 1, msg = "Success" });
                }
            });
        }

        public JsonResult ChangeMoreTaskState(string poststr)
        {
            return this.Visit(Core.EnumUserRole.Admin, () =>
            {
                System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<PostChangeModel> post = new List<PostChangeModel>();
                post = jss.Deserialize<List<PostChangeModel>>(poststr);
                tb_command_dal dal = new tb_command_dal();
                tb_task_dal taskDal = new tb_task_dal();
                using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                {
                    PubConn.Open();
                    foreach (PostChangeModel m in post)
                    {
                        m.state = m.state == 0 ? 1 : 0;
                        if (taskDal.CheckTaskState(PubConn, m.id) == m.state)
                        {
                            string msg = m.state == 1 ? "已开启" : "已关闭";
                            return Json(new { code = -1, msg = msg });
                        }
                        else
                        {
                            tb_command_model c = new tb_command_model()
                            {
                                command = "",
                                commandcreatetime = DateTime.Now,
                                commandname = m.state == (int)EnumTaskCommandName.StartTask ? EnumTaskCommandName.StartTask.ToString() : EnumTaskCommandName.StopTask.ToString(),
                                taskid = m.id,
                                nodeid = m.nodeid,
                                commandstate = (int)EnumTaskCommandState.None
                            };
                            dal.Add(PubConn, c);
                        }
                    }
                    return Json(new { code = 1, data = post });
                }
            });
        }

        public JsonResult CheckTaskState(int id, int state)
        {
            return this.Visit(EnumUserRole.None, () =>
            {
                tb_task_dal dal = new tb_task_dal();
                using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                {
                    PubConn.Open();
                    int taskstate = dal.CheckTaskState(PubConn, id);
                    if (taskstate == state)
                    {
                        return Json(new { code = 1, msg = "Success" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { code = -1, msg = "" }, JsonRequestBehavior.AllowGet);
                    }
                }
            });
        }

        public JsonResult Delete(int id)
        {
            return this.Visit(Core.EnumUserRole.Admin, () =>
            {
                try
                {
                    tb_task_dal dal = new tb_task_dal();
                    using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                    {
                        PubConn.Open();
                        bool state = dal.DeleteOneTask(PubConn, id);
                        return Json(new { code = 1, state = state });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { code = -1, msg = ex.Message });
                }
            });
        }

        public JsonResult Uninstall(int id)
        {
            return this.Visit(Core.EnumUserRole.Admin, () =>
            {
                try
                {
                    tb_command_dal commanddal = new tb_command_dal();
                    tb_task_dal dal = new tb_task_dal();
                    using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                    {
                        PubConn.Open();
                        var taskmodel = dal.Get(PubConn, id);
                        dal.UpdateTaskState(PubConn, id,(int)Core.EnumTaskState.Stop);

                        tb_command_model m = new tb_command_model()
                        {
                            command = "",
                            commandcreatetime = DateTime.Now,
                            commandname =  EnumTaskCommandName.UninstallTask.ToString(),
                            taskid = id,
                            nodeid = taskmodel.nodeid,
                            commandstate = (int)EnumTaskCommandState.None
                        };
                        commanddal.Add(PubConn, m);

                        return Json(new { code = 1 });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { code = -1, msg = ex.Message });
                }
            });
        }

        public ActionResult Cron()
        {
            return View();
        }

        public ActionResult CustomCorn()
        {
            return View();
        }
    }
}
