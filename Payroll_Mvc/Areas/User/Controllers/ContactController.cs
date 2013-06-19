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
    public class ContactController : AsyncController
    {
        //
        // GET: /User/Contact/

        public ActionResult Index()
        {
            object id = Session["employee_id"];
            Employeecontact employee_contact = EmployeecontactHelper.Find(id);

            return View(employee_contact);
        }

        public async Task<JsonResult> Update(FormCollection fc)
        {
            ISession se = NHibernateHelper.CurrentSession;

            object id = Session["employee_id"];
            Employeecontact oc = EmployeecontactHelper.Find(id);

            Employee o = se.Get<Employee>(id);
            oc = EmployeecontactHelper.GetObject(o, fc);

            Dictionary<string, object> err = oc.IsValid();

            if (err != null)
                return Json(err, JsonRequestBehavior.AllowGet);

            await Task.Run(() =>
                {
                    using (ITransaction tx = se.BeginTransaction())
                    {
                        se.SaveOrUpdate(oc);
                        tx.Commit();
                    }
                });

            return Json(new Dictionary<string, object>
            {
                { "success", 1 },
                { "message", "Contact Details was successfully updated." }
            },
            JsonRequestBehavior.AllowGet);
        }
    }
}
