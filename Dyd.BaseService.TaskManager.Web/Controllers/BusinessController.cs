using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dyd.BaseService.TaskManager.Web.Controllers
{
    public class BusinessController : Controller
    {
        //
        // GET: /Business/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OpreationRecord()
        {
            return View();
        }

        public ActionResult Log()
        {
            return View();
        }
    }
}
