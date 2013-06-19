using System.Web.Mvc;

namespace Payroll_Mvc.Areas.User
{
    public class UserAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "User";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "User_index",
                "user",
                new { controller = "User", action = "Index" }
            );

            context.MapRoute(
                "User_hourlypayrollchart",
                "user/hourly/chart/{action}",
                new { controller = "HourlyPayrollChart", action = "Index" }
            );

            context.MapRoute(
                "User_totalworkhourschart",
                "user/workhours/chart/{action}",
                new { controller = "TotalWorkHoursChart", action = "Index" }
            );

            context.MapRoute(
                "User_overtimechart",
                "user/overtime/chart/{action}",
                new { controller = "OvertimeChart", action = "Index" }
            );

            context.MapRoute(
                "User_payslip",
                "user/payslip/slip/{month}/{year}",
                new { controller = "Payslip", action = "Payslip" }
            );

            context.MapRoute(
                "User_default",
                "User/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
