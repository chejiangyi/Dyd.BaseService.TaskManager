using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dyd.BaseService.TaskManager.Domain.Model;
using Dyd.BaseService.TaskManager.Domain.Dal;
using XXF.Db;
using Dyd.BaseService.TaskManager.Web.Models;
using XXF.Extensions;

namespace Dyd.BaseService.TaskManager.Web.Controllers
{
    [AuthorityCheck]
    public class UserController : BaseWebController
    {
        //
        // GET: /Developers/

        public ActionResult Index()
        {
            return this.Visit(Core.EnumUserRole.Admin,() =>
            {
                using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                {
                    PubConn.Open();
                    List<tb_user_model> Model = new tb_user_dal().GetAllUsers(PubConn);
                    return View(Model);
                }
            });
        }

        public ActionResult Add(int? userid)
        {
            return this.Visit(Core.EnumUserRole.Admin, () =>
            {
                if (userid == null)
                    return View();
                using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                {
                    PubConn.Open();
                    tb_user_dal dal = new tb_user_dal();
                    
                    var model = dal.Get(PubConn,userid.Value);
                    return View(model);
                }
            });
        }

        [HttpPost]
        public ActionResult Add(tb_user_model model)
        {
            return this.Visit(Core.EnumUserRole.Admin, () =>
            {
                using (DbConn PubConn = DbConfig.CreateConn(Config.TaskConnectString))
                {
                    PubConn.Open();
                    tb_user_dal dal = new tb_user_dal();
                    model.usercreatetime = DateTime.Now;
                    model.usertel = model.usertel.NullToEmpty();
                    model.useremail = model.useremail.NullToEmpty();
                    if (model.id == 0)
                        dal.Add(PubConn, model);
                    else
                        dal.Edit(PubConn, model);
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
                    tb_user_dal dal = new tb_user_dal();
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
