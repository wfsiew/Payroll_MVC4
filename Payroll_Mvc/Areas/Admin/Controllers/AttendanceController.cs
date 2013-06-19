using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

using Domain.Model;
using Payroll_Mvc.Models;
using Payroll_Mvc.Helpers;
using Payroll_Mvc.Attributes;
using Payroll_Mvc.Areas.Admin.Models;

namespace Payroll_Mvc.Areas.Admin.Controllers
{
    [Authorize]
    [AuthAdmin]
    public class AttendanceController : AsyncController
    {
        //
        // GET: /Admin/Attendance/

        public async Task<ActionResult> Index()
        {
            ListModel<Attendance> l = null;
            l = await AttendanceHelper.GetAll();

            return View(l);
        }

        public async Task<ActionResult> List()
        {
            DateTime work_date = CommonHelper.GetDateTime(Request["work_date"]);
            string employee = CommonHelper.GetValue(Request["employee"]);

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
                l = await AttendanceHelper.GetAll(pgnum, pgsize, sort);

            else
                l = await AttendanceHelper.GetFilterBy(filters, pgnum, pgsize, sort);

            return View("_list", l);
        }
    }
}
