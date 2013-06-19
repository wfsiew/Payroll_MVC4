using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Payroll_Mvc.Attributes;

namespace Payroll_Mvc.Areas.Admin.Controllers
{
    [Authorize]
    [AuthAdmin]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/Index/

        public ActionResult Index()
        {
            return View();
        }
    }
}
