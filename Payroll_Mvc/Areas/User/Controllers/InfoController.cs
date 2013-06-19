using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

using NHibernate;
using Domain.Model;
using Payroll_Mvc.Helpers;
using Payroll_Mvc.Attributes;

namespace Payroll_Mvc.Areas.User.Controllers
{
    [Authorize]
    [AuthUser]
    public class InfoController : AsyncController
    {
        //
        // GET: /User/Info/

        public ActionResult Index()
        {
            ISession se = NHibernateHelper.CurrentSession;

            object id = Session["employee_id"];
            Employee employee = se.Get<Employee>(id);
            ViewBag.user = employee.User;

            return View(employee);
        }

        public async Task<JsonResult> Update(FormCollection fc)
        {
            ISession se = NHibernateHelper.CurrentSession;

            object id = Session["employee_id"];
            Employee o = se.Get<Employee>(id);

            o = await EmployeeHelper.GetObject(se, o, fc);

            Dictionary<string, object> err = o.IsValid(se);

            if (err != null)
                return Json(err, JsonRequestBehavior.AllowGet);

            await Task.Run(() =>
                {
                    using (ITransaction tx = se.BeginTransaction())
                    {
                        se.SaveOrUpdate(o);
                        tx.Commit();
                    }
                });

            return Json(new Dictionary<string, object>
            {
                { "success", 1 },
                { "message", "Personal Details was successfully updated." }
            },
            JsonRequestBehavior.AllowGet);
        }
    }
}
