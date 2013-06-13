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
                "Admin_attendance",
                "admin/att/{action}",
                new { controller = "Attendance", action = "Index" }
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
