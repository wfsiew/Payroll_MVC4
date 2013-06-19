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
    public class QualificationController : AsyncController
    {
        //
        // GET: /User/Qualifications/

        public ActionResult Index()
        {
            object id = Session["employee_id"];
            Employeequalification employee_qualification = EmployeequalificationHelper.Find(id);

            return View(employee_qualification);
        }

        public async Task<JsonResult> Update(FormCollection fc)
        {
            ISession se = NHibernateHelper.CurrentSession;

            object id = Session["employee_id"];
            Employeequalification oq = EmployeequalificationHelper.Find(id);

            Employee o = se.Get<Employee>(id);
            oq = EmployeequalificationHelper.GetObject(o, fc);

            Dictionary<string, object> err = oq.IsValid();

            if (err != null)
                return Json(err, JsonRequestBehavior.AllowGet);

            await Task.Run(() =>
                {
                    using (ITransaction tx = se.BeginTransaction())
                    {
                        se.SaveOrUpdate(oq);
                        tx.Commit();
                    }
                });

            return Json(new Dictionary<string, object>
            {
                { "success", 1 },
                { "message", "Qualifications was successfully updated." }
            },
            JsonRequestBehavior.AllowGet);
        }
    }
}
