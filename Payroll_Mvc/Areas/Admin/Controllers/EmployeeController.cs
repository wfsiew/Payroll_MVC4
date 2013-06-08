using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Domain.Model;
using NHibernate;
using Payroll_Mvc.Models;
using Payroll_Mvc.Helpers;
using Payroll_Mvc.Areas.Admin.Models;

namespace Payroll_Mvc.Areas.Admin.Controllers
{
    public class EmployeeController : Controller
    {
        //
        // GET: /Admin/Employee/

        public ActionResult Index()
        {
            ListModel<Employee> l = null;

            ISession se = NHibernateHelper.CurrentSession;
            l = EmployeeHelper.GetAll();
            ViewBag.employmentstatus = se.QueryOver<Employmentstatus>()
                .OrderBy(x => x.Name).Asc.List();
            ViewBag.designation = se.QueryOver<Designation>()
                .OrderBy(x => x.Title).Asc.List();
            ViewBag.dept = se.QueryOver<Department>()
                .OrderBy(x => x.Name).Asc.List();

            return View(l);
        }

        public ActionResult List()
        {
            string employee = string.IsNullOrEmpty(Request["employee"]) ? "" : Request["employee"];
            string staff_id = string.IsNullOrEmpty(Request["staff_id"]) ? "" : Request["staff_id"];
            int employment_status = string.IsNullOrEmpty(Request["employment_status"]) ? 0 : Convert.ToInt32(Request["employment_status"]);
            int designation = string.IsNullOrEmpty(Request["designation"]) ? 0 : Convert.ToInt32(Request["designation"]);
            int dept = string.IsNullOrEmpty(Request["dept"]) ? 0 : Convert.ToInt32(Request["dept"]);
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
                l = EmployeeHelper.GetAll(pgnum, pgsize, sort);

            else
                l = EmployeeHelper.GetFilterBy(filters, pgnum, pgsize, sort);

            return View("_list", l);
        }

        public ActionResult New()
        {
            ISession se = NHibernateHelper.CurrentSession;

            EmployeeView e = new EmployeeView
            {
                Employee = new Employee(),
                Employeecontact = new Employeecontact(),
                Employeejob = new Employeejob(),
                Employeequalification = new Employeequalification(),
                Employeesalary = new Employeesalary()
            };
            ViewBag.form_id = "add-form";
            ViewBag.users = se.QueryOver<User>()
                .OrderBy(x => x.Username).Asc.List();
            ViewBag.designations = se.QueryOver<Designation>()
                .OrderBy(x => x.Title).Asc.List();
            ViewBag.employment_statuses = se.QueryOver<Employmentstatus>()
                .OrderBy(x => x.Name).Asc.List();
            ViewBag.job_categories = se.QueryOver<Jobcategory>()
                .OrderBy(x => x.Name).Asc.List();
            ViewBag.departments = se.QueryOver<Department>()
                .OrderBy(x => x.Name).Asc.List();

            return View("_form", e);
        }
    }
}
