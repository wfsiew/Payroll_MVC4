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
                Employeesalary = new Employeesalary(),
                Employeequalification = new Employeequalification()
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

        [HttpPost]
        public JsonResult Create(FormCollection fc)
        {
            Employee o = EmployeeHelper.GetObject(fc);

            bool b1 = EmployeecontactHelper.IsEmptyParams(fc);
            Employeecontact oc = EmployeecontactHelper.GetObject(o, fc);

            bool b2 = EmployeejobHelper.IsEmptyParams(fc);
            Employeejob oej = EmployeejobHelper.GetObject(o, fc);

            bool b3 = EmployeesalaryHelper.IsEmptyParams(fc);
            Employeesalary osa = EmployeesalaryHelper.GetObject(o, fc);

            bool b4 = EmployeequalificationHelper.IsEmptyParams(fc);
            Employeequalification oq = EmployeequalificationHelper.GetObject(o, fc);

            ISession se = NHibernateHelper.CurrentSession;

            Dictionary<string, object> employeeErrors = o.IsValid(se);
            Dictionary<string, object> employeeContactErrors = null;
            Dictionary<string, object> employeeJobErrors = null;
            Dictionary<string, object> employeeSalaryErrors = null;
            Dictionary<string, object> employeeQualificationErrors = null;

            bool v1 = employeeErrors == null;
            bool v2 = b1 ? true : (employeeContactErrors = oc.IsValid()) == null;
            bool v3 = b2 ? true : (employeeJobErrors = oej.IsValid()) == null;
            bool v4 = b3 ? true : (employeeSalaryErrors = osa.IsValid()) == null;
            bool v5 = b4 ? true : (employeeQualificationErrors = oq.IsValid()) == null;

            if (!v1 || !v2 || !v3 || !v4 || !v5)
            {
                Dictionary<string, object> err = new Dictionary<string, object>
                {
                    { "error", 1 },
                    { "employee", CommonHelper.GetErrors(employeeErrors) },
                    { "employee_contact", CommonHelper.GetErrors(employeeContactErrors) },
                    { "employee_job", CommonHelper.GetErrors(employeeJobErrors) },
                    { "employee_salary", CommonHelper.GetErrors(employeeSalaryErrors) },
                    { "employee_qualification", CommonHelper.GetErrors(employeeQualificationErrors) }
                };
                return Json(err, JsonRequestBehavior.AllowGet);
            }

            using (ITransaction tx = se.BeginTransaction())
            {
                se.SaveOrUpdate(o);
                oc.Id = o.Id;
                oej.Id = o.Id;
                osa.Id = o.Id;
                oq.Id = o.Id;

                if (!b1)
                    se.SaveOrUpdate(oc);

                if (!b2)
                    se.SaveOrUpdate(oej);

                if (!b3)
                    se.SaveOrUpdate(osa);

                if (!b4)
                    se.SaveOrUpdate(oq);

                tx.Commit();
            }

            return Json(new Dictionary<string, object>
            {
                { "success", 1 },
                { "message", "Employee was successfully added." }
            },
            JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(Guid id)
        {
            ISession se = NHibernateHelper.CurrentSession;
            Employee o = se.Get<Employee>(id);
            Employeecontact oc = o.Employeecontact;
            Employeejob oej = o.Employeejob;
            Employeesalary osa = o.Employeesalary;
            Employeequalification oq = o.Employeequalification;

            EmployeeView e = new EmployeeView
            {
                Employee = o,
                Employeecontact = oc == null ? new Employeecontact() : oc,
                Employeejob = oej == null ? new Employeejob() : oej,
                Employeesalary = osa == null ? new Employeesalary() : osa,
                Employeequalification = oq == null ? new Employeequalification() : oq
            };

            ViewBag.form_id = "edit-form";
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

        [HttpPost]
        public JsonResult Update(Guid id, FormCollection fc)
        {
            ISession se = NHibernateHelper.CurrentSession;

            Employee o = se.Get<Employee>(id);
            Employeecontact oc = EmployeecontactHelper.GetObject(o, fc);
            Employeejob oej = EmployeejobHelper.GetObject(o, fc); ;
            Employeesalary osa = EmployeesalaryHelper.GetObject(o, fc);
            Employeequalification oq = EmployeequalificationHelper.GetObject(o, fc);

            bool b1 = EmployeecontactHelper.IsEmptyParams(fc);
            bool b2 = EmployeejobHelper.IsEmptyParams(fc);
            bool b3 = EmployeesalaryHelper.IsEmptyParams(fc);
            bool b4 = EmployeequalificationHelper.IsEmptyParams(fc);

            Dictionary<string, object> employeeErrors = o.IsValid(se);
            Dictionary<string, object> employeeContactErrors = null;
            Dictionary<string, object> employeeJobErrors = null;
            Dictionary<string, object> employeeSalaryErrors = null;
            Dictionary<string, object> employeeQualificationErrors = null;

            bool v1 = employeeErrors == null;
            bool v2 = b1 ? true : (employeeContactErrors = oc.IsValid()) == null;
            bool v3 = b2 ? true : (employeeJobErrors = oej.IsValid()) == null;
            bool v4 = b3 ? true : (employeeSalaryErrors = osa.IsValid()) == null;
            bool v5 = b4 ? true : (employeeQualificationErrors = oq.IsValid()) == null;

            if (!v1 || !v2 || !v3 || !v4 || !v5)
            {
                Dictionary<string, object> err = new Dictionary<string, object>
                {
                    { "error", 1 },
                    { "employee", CommonHelper.GetErrors(employeeErrors) },
                    { "employee_contact", CommonHelper.GetErrors(employeeContactErrors) },
                    { "employee_job", CommonHelper.GetErrors(employeeJobErrors) },
                    { "employee_salary", CommonHelper.GetErrors(employeeSalaryErrors) },
                    { "employee_qualification", CommonHelper.GetErrors(employeeQualificationErrors) }
                };
                return Json(err, JsonRequestBehavior.AllowGet);
            }

            using (ITransaction tx = se.BeginTransaction())
            {
                se.SaveOrUpdate(o);
                se.SaveOrUpdate(oc);
                se.SaveOrUpdate(oej);
                se.SaveOrUpdate(osa);
                se.SaveOrUpdate(oq);

                tx.Commit();
            }

            return Json(new Dictionary<string, object>
            {
                { "success", 1 },
                { "message", "Employee was successfully updated." }
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(FormCollection fc)
        {
            string employee = string.IsNullOrEmpty(Request["employee"]) ? "" : Request["employee"];
            string staff_id = string.IsNullOrEmpty(Request["staff_id"]) ? "" : Request["staff_id"];
            int employment_status = string.IsNullOrEmpty(Request["employment_status"]) ? 0 : Convert.ToInt32(Request["employment_status"]);
            int designation = string.IsNullOrEmpty(Request["designation"]) ? 0 : Convert.ToInt32(Request["designation"]);
            int dept = string.IsNullOrEmpty(Request["dept"]) ? 0 : Convert.ToInt32(Request["dept"]);
            int pgnum = string.IsNullOrEmpty(Request["pgnum"]) ? 1 : Convert.ToInt32(Request["pgnum"]);
            int pgsize = string.IsNullOrEmpty(Request["pgsize"]) ? 0 : Convert.ToInt32(Request["pgsize"]);
            string ids = fc.Get("id[]");
            string[] idlist = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            string itemscount = null;

            Dictionary<string, object> filters = new Dictionary<string, object>
            {
                { "employee", employee },
                { "staff_id", staff_id },
                { "employment_status", employment_status },
                { "designation", designation },
                { "dept", dept }
            };

            ISession se = NHibernateHelper.CurrentSession;

            using (ITransaction tx = se.BeginTransaction())
            {
                se.CreateQuery("delete from Employee where id in (:idlist)")
                    .SetParameterList("idlist", idlist)
                    .ExecuteUpdate();
                se.CreateQuery("delete from Employeecontact where id in (:idlist)")
                    .SetParameterList("idlist", idlist)
                    .ExecuteUpdate();
                se.CreateQuery("delete from Employeejob where id in (:idlist)")
                    .SetParameterList("idlist", idlist)
                    .ExecuteUpdate();
                se.CreateQuery("delete from Employeesalary where id in (:idlist)")
                    .SetParameterList("idlist", idlist)
                    .ExecuteUpdate();
                se.CreateQuery("delete from Employeequalification where id in (:idlist)")
                    .SetParameterList("idlist", idlist)
                    .ExecuteUpdate();

                tx.Commit();
            }

            if (string.IsNullOrEmpty(employee) && string.IsNullOrEmpty(staff_id) && employment_status == 0 &&
                designation == 0 && dept == 0)
                itemscount = EmployeeHelper.GetItemMessage(null, pgnum, pgsize);

            else
                itemscount = EmployeeHelper.GetItemMessage(filters, pgnum, pgsize);

            return Json(new Dictionary<string, object>
            {
                { "success", 1 },
                { "itemscount", itemscount },
                { "message", string.Format("{0} employee(s) was successfully deleted.", idlist.Length) }
            },
            JsonRequestBehavior.AllowGet);
        }
    }
}
