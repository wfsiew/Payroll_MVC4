using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

using NHibernate;
using Domain.Model;
using Payroll_Mvc.Helpers;
using Payroll_Mvc.Areas.User.Models;
using Payroll_Mvc.Attributes;

namespace Payroll_Mvc.Areas.User.Controllers
{
    [Authorize]
    [AuthUser]
    public class SalaryController : AsyncController
    {
        //
        // GET: /User/Salary/

        public async Task<ActionResult> Index()
        {
            ISession se = NHibernateHelper.CurrentSession;

            SalaryView o = new SalaryView();

            object id = Session["employee_id"];
            Employee employee = se.Get<Employee>(id);
            o.Employeesalary = EmployeesalaryHelper.Find(id);
            double adjustment = await SalaryadjustmentHelper.GetSalaryAdjustment(new Dictionary<string, object>
            {
                { "staff_id", employee.Staffid },
                { "year", DateTime.Now.Year }
            });
            o.BasicPay = o.Employeesalary.Salary + adjustment;

            return View(o);
        }
    }
}
