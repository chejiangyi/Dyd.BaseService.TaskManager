using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dyd.BaseService.TaskManager.Core;
using Dyd.BaseService.TaskManager.Core.Net;
using Dyd.BaseService.TaskManager.Web.Models;

namespace Dyd.BaseService.TaskManager.Web.Controllers
{
    public class OpenApiController : BaseWebController
    {
        //
        // GET: /Api/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetNodeConfigInfo()
        {

            NodeAppConfigInfo nodeinfo = new NodeAppConfigInfo();
            nodeinfo.NodeID = Common.GetAvailableNode();
            nodeinfo.TaskDataBaseConnectString =  StringDESHelper.EncryptDES(Config.TaskConnectString,"dyd88888888");
            return Json( new  { code = 1, msg = "", data = nodeinfo, total = 0 } , JsonRequestBehavior.AllowGet);
        }

        public string Ping()
        {
            return "ok";
        }
    }
}
