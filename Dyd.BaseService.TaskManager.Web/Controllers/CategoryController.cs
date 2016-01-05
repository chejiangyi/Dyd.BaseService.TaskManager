using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dyd.BaseService.TaskManager.Domain.Model;
using Dyd.BaseService.TaskManager.Domain.Dal;
using XXF.Db;
using Dyd.BaseService.TaskManager.Web.Models;

namespace Dyd.BaseService.TaskManager.Web.Controllers
{
    [AuthorityCheck]
    public class CategoryController : BaseWebController
    {
        //
        // GET: /Category/

        public ActionResult Index(string keyword)
        {
            return this.Visit(Core.EnumUserRole.Admin, () =>
            {
                using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                {
                    PubConn.Open();
                    tb_category_dal dal = new tb_category_dal();
                    List<tb_category_model> model = dal.GetList(PubConn, keyword);
                    return View(model);
                }
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
        public ActionResult Add(tb_category_model model)
        {
            return this.Visit(Core.EnumUserRole.Admin, () =>
            {
                using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                {
                    PubConn.Open();
                    tb_category_dal dal = new tb_category_dal();
                    dal.Add(PubConn, model.categoryname);
                    return RedirectToAction("index");
                }
            });
        }

        public JsonResult Update(tb_category_model model)
        {
            return this.Visit(Core.EnumUserRole.Admin, () =>
            {
                using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                {
                    PubConn.Open();
                    tb_category_dal dal = new tb_category_dal();
                    dal.Update(PubConn, model);
                    return Json(new { code = 1, msg = "Success" });
                }
            });
        }

        public JsonResult Delete(int id)
        {
            return this.Visit(Core.EnumUserRole.Admin, () =>
            {
                try
                {
                    tb_category_dal dal = new tb_category_dal();
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
