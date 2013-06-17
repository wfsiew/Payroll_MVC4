using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

using Domain.Model;
using NHibernate;
using Payroll_Mvc.Models;
using Payroll_Mvc.Helpers;
using Payroll_Mvc.Areas.Admin.Models;

namespace Payroll_Mvc.Areas.Admin.Controllers
{
    public class PayslipController : AsyncController
    {
        //
        // GET: /Admin/Payslip/

        public async Task<ActionResult> Index()
        {
            ListModel<Employee> l = null;

            ISession se = NHibernateHelper.CurrentSession;
            l = await EmployeeHelper.GetAll();
            ViewBag.employmentstatus = se.QueryOver<Employmentstatus>()
                .OrderBy(x => x.Name).Asc.List();
            ViewBag.designation = se.QueryOver<Designation>()
                .OrderBy(x => x.Title).Asc.List();
            ViewBag.dept = se.QueryOver<Department>()
                .OrderBy(x => x.Name).Asc.List();

            return View(l);
        }

        public async Task<ActionResult> List()
        {
            string employee = CommonHelper.GetValue(Request["employee"]);
            string staff_id = CommonHelper.GetValue(Request["staff_id"]);
            int employment_status = CommonHelper.GetValue<int>(Request["employment_status"], 0);
            int designation = CommonHelper.GetValue<int>(Request["designation"], 0);
            int dept = CommonHelper.GetValue<int>(Request["dept"], 0);
            int pgnum = string.IsNullOrEmpty(Request["pgnum"]) ? 1 : Convert.ToInt32(Request["pgnum"]);
            int pgsize = string.IsNullOrEmpty(Request["pgsize"]) ? 0 : Convert.ToInt32(Request["pgsize"]);
            string sortcolumn = string.IsNullOrEmpty(Request["sortcolumn"]) ? EmployeeHelper.DEFAULT_SORT_COLUMN : Request["sortcolumn"];
            string sortdir = string.IsNullOrEmpty(Request["sortdir"]) ? EmployeeHelper.DEFAULT_SORT_DIR : Request["sortdir"];

            Sort sort = new Sort(sortcolumn, sortdir);

            Dictionary<string, object> filters = new Dictionary<string, object>
            {
                { "employee", employee },
                { "staff_id", staff_id },
                { "employment_status", employment_status },
                { "designation", designation },
                { "dept", dept }
            };

            ListModel<Employee> l = null;

            if (string.IsNullOrEmpty(employee) && string.IsNullOrEmpty(staff_id) && employment_status == 0 &&
                designation == 0 && dept == 0)
                l = await EmployeeHelper.GetAll(pgnum, pgsize, sort);

            else
                l = await EmployeeHelper.GetFilterBy(filters, pgnum, pgsize, sort);

            return View("_list", l);
        }

        public async Task<ActionResult> Payslip(Guid id, int month, int year)
        {
            ISession se = NHibernateHelper.CurrentSession;

            Employee employee = se.Get<Employee>(id);
            Employeesalary employee_salary = employee.Employeesalary;

            PayslipModel o = new PayslipModel();
            o.Period = string.Format("{0}-{1}", month, year);
            o.Employee = employee;
            o.EmployeeSalary = employee_salary;

            if (employee_salary == null)
            {
                o.EmployeeSalary = new Employeesalary();
                return View("payslip_monthly", o);
            }

            else
            {
                if (employee_salary.Paytype == 1)
                {
                    Dictionary<string, object> filters = new Dictionary<string, object>
                    {
                        { "year", year },
                        { "month", month },
                        { "staff_id", employee.Staffid }
                    };

                    o.TotalOvertime = await PayslipHelper.GetTotalOvertime(filters);
                    o.TotalOvertimeEarnings = await PayslipHelper.GetTotalOvertimeEarnings(filters, o.TotalOvertime);
                    o.Adjustment = await SalaryadjustmentHelper.GetSalaryAdjustment(filters);

                }
            }

            return null;
        }
    }
}
