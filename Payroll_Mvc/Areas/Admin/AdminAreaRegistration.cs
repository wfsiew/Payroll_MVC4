using System.Web.Mvc;
using Payroll_Mvc.Areas.Admin.Controllers;

namespace Payroll_Mvc.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_index",
                "admin",
                new { controller = "Admin", action = "Index" }
            );

            context.MapRoute(
                "Admin_attendance",
                "admin/att/{action}",
                new { controller = "Attendance", action = "Index" }
            );

            context.MapRoute(
                "Admin_overtimerate",
                "admin/overtime/rate/{action}",
                new { controller = "OvertimeRate", action = "Index" }
            );

            context.MapRoute(
                "Admin_empstatus",
                "admin/empstatus/{action}/{id}",
                new { controller = "EmploymentStatus", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_jobcat",
                "admin/jobcat/{action}/{id}",
                new { controller = "JobCategory", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_dept",
                "admin/dept/{action}/{id}",
                new { controller = "Department", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_hourlypayrollchart",
                "admin/hourly/chart/{action}",
                new { controller = "HourlyPayrollChart", action = "Index" }
            );

            context.MapRoute(
                "Admin_totalworkhourschart",
                "admin/workhours/chart/{action}",
                new { controller = "TotalWorkHoursChart", action = "Index" }
            );

            context.MapRoute(
                "Admin_salaryadj",
                "admin/salaryadj/{action}/{id}",
                new { controller = "SalaryAdjustment", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_overtimechart",
                "admin/overtime/chart/{action}",
                new { controller = "OvertimeChart", action = "Index" }
            );

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
