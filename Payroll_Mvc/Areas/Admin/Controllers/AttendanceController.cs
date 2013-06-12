using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Domain.Model;
using Payroll_Mvc.Models;
using Payroll_Mvc.Helpers;
using Payroll_Mvc.Areas.Admin.Models;

namespace Payroll_Mvc.Areas.Admin.Controllers
{
    public class AttendanceController : Controller
    {
        //
        // GET: /Admin/Attendance/

        public ActionResult Index()
        {
            ListModel<Attendance> l = null;
            l = AttendanceHelper.GetAll();

            return View(l);
        }

        public ActionResult List()
        {
            string _work_date = Request["work_date"];
            string employee = string.IsNullOrEmpty(Request["employee"]) ? "" : Request["employee"];

            DateTime work_date = string.IsNullOrEmpty(_work_date) ? default(DateTime) : CommonHelper.GetDateTime(_work_date);

            int pgnum = string.IsNullOrEmpty(Request["pgnum"]) ? 1 : Convert.ToInt32(Request["pgnum"]);
            int pgsize = string.IsNullOrEmpty(Request["pgsize"]) ? 0 : Convert.ToInt32(Request["pgsize"]);
            string sortcolumn = string.IsNullOrEmpty(Request["sortcolumn"]) ? EmployeeHelper.DEFAULT_SORT_COLUMN : Request["sortcolumn"];
            string sortdir = string.IsNullOrEmpty(Request["sortdir"]) ? EmployeeHelper.DEFAULT_SORT_DIR : Request["sortdir"];

            Sort sort = new Sort(sortcolumn, sortdir);

            Dictionary<string, object> filters = new Dictionary<string, object>
            {
                { "work_date", work_date },
                { "employee", employee }
            };

            ListModel<Attendance> l = null;

            if (work_date == default(DateTime) && string.IsNullOrEmpty(employee))
                l = AttendanceHelper.GetAll(pgnum, pgsize, sort);

            else
                l = AttendanceHelper.GetFilterBy(filters, pgnum, pgsize, sort);

            return View("_list", l);
        }
    }
}
