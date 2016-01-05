using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XXF.Db;
using Dyd.BaseService.TaskManager.Web.Models;
using Dyd.BaseService.TaskManager.Domain.Dal;
using Dyd.BaseService.TaskManager.Domain.Model;
using Webdiyer.WebControls.Mvc;
using Dyd.BaseService.TaskManager.Core.Net;
using Dyd.BaseService.TaskManager.Core;

namespace Dyd.BaseService.TaskManager.Web.Controllers
{
    [AuthorityCheck]
    public class NodeController : BaseWebController
    {
        //
        // GET: /Node/

        public ActionResult Index(string keyword, string CStime, string CEtime, int pagesize = 10, int pageindex = 1)
        {
            return this.Visit(Core.EnumUserRole.Admin, () =>
            {
                ViewBag.sqldatetimenow = DateTime.Now;
                tb_node_dal dal = new tb_node_dal();
                Webdiyer.WebControls.Mvc.PagedList<tb_node_model> pageList = null;
                int count = 0;
                using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                {
                    PubConn.Open();
                    List<tb_node_model> List = dal.GetList(PubConn, keyword, CStime, CEtime, pagesize, pageindex, out count);
                    pageList = new PagedList<tb_node_model>(List, pageindex, pagesize, count);
                    ViewBag.sqldatetimenow = PubConn.GetServerDate();
                }
                return View(pageList);
            });
        }

        public ActionResult Add()
        {
            return this.Visit(Core.EnumUserRole.Admin, () =>
            {
                return View();
            });
        }

        [HttpPost]
        public ActionResult Add(tb_node_model model)
        {
            return this.Visit(Core.EnumUserRole.Admin, () =>
            {
                tb_node_dal Dal = new tb_node_dal();
                model.nodecreatetime = DateTime.Now;
                model.nodelastupdatetime = DateTime.Now;
                using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                {
                    PubConn.Open();
                    Dal.AddOrUpdate(PubConn, model);
                }
                return RedirectToAction("index");
            });
        }

        public ActionResult Update(int id)
        {
            return this.Visit(Core.EnumUserRole.Admin, () =>
            {
                tb_node_dal dal = new tb_node_dal();
                using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                {
                    PubConn.Open();
                    tb_node_model model = dal.GetOneNode(PubConn, id);
                    return View(model);
                }
            });
        }

        [HttpPost]
        public ActionResult Update(tb_node_model model)
        {
            return this.Visit(Core.EnumUserRole.Admin, () =>
            {
                tb_node_dal Dal = new tb_node_dal();
                model.nodecreatetime = DateTime.Now;
                model.nodelastupdatetime = DateTime.Now;
                using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                {
                    PubConn.Open();
                    Dal.Update(PubConn, model);
                }
                return RedirectToAction("index");
            });
        }

        public JsonResult Delete(int id)
        {
            return this.Visit(Core.EnumUserRole.Admin, () =>
            {
                try
                {
                    tb_node_dal dal = new tb_node_dal();
                    using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                    {
                        PubConn.Open();
                        bool state = dal.DeleteOneNode(PubConn, id);
                        return Json(new { code = 1, state = state });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { code = -1, msg = ex.Message });
                }
            });
        }

        
    }
}
