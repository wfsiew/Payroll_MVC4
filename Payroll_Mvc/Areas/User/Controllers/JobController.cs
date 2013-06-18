using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

using NHibernate;
using Domain.Model;
using Payroll_Mvc.Helpers;

namespace Payroll_Mvc.Areas.User.Controllers
{
    public class JobController : Controller
    {
        //
        // GET: /User/Job/

        public ActionResult Index()
        {
            object id = Session["employee_id"];
            Employeejob employee_job = EmployeejobHelper.Find(id);

            return View(employee_job);
        }
    }
}
