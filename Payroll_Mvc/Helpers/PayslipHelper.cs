using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

using NHibernate;
using NHibernate.SqlCommand;
using NHibernate.Criterion;
using NHibernate.Criterion.Lambda;
using Domain.Model;
using Payroll_Mvc.Models;
using Payroll_Mvc.Areas.Admin.Models;

namespace Payroll_Mvc.Helpers
{
    public class PayslipHelper
    {
        public static double GetTotalDeductions(Employeesalary employee_salary)
        {
            if (employee_salary == null)
                return 0;

            return employee_salary.Epf + employee_salary.Socso + employee_salary.Incometax;
        }

        public static double GetTotalEarnings(Employeesalary employee_salary, double adjustment, double total_overtime_earnings)
        {
            if (employee_salary == null)
                return 0;

            double amt = employee_salary.Salary + adjustment;

            return amt + employee_salary.Allowance + total_overtime_earnings;
        }

        public static double GetNettSalary(double earnings, double deductions)
        {
            return earnings - deductions;
        }

        public static async Task<double[]> GetTotalEarningsHourly(Employeesalary employee_salary, Dictionary<string, object> filters)
        {
            double total_hours = await AttendanceHelper.GetTotalHours(filters);
            double rate = await PayrateHelper.GetPayRate(filters);

            double earnings = (total_hours * rate) + employee_salary.Allowance;

            double[] a = new double[] { earnings, total_hours, rate };
            return a;
        }

        public static double GetNettSalaryHourly(double earnings, double deductions)
        {
            return earnings - deductions;
        }

        public static async Task<double> GetTotalOvertime(Dictionary<string, object> filters)
        {
            int year = (int)filters["year"];
            int month = (int)filters["month"];
            string staff_id = Convert.ToString(filters["staff_id"]);

            ISession se = NHibernateHelper.CurrentSession;

            ICriteria cr = se.CreateCriteria<Attendance>();

            cr.Add(Restrictions.Eq("Staffid", staff_id));

            IProjection monthProjection = Projections.SqlFunction("month", NHibernateUtil.Int32, Projections.Property("Workdate"));
            cr.Add(Restrictions.Eq(monthProjection, month));

            IProjection yearProjection = Projections.SqlFunction("year", NHibernateUtil.Int32, Projections.Property("Workdate"));
            cr.Add(Restrictions.Eq(yearProjection, year));

            IList<Attendance> list = await Task.Run(() => { return cr.List<Attendance>(); });

            double duration = 0;

            await Task.Run(() =>
                {
                    foreach (Attendance o in list)
                    {
                        DateTime to = o.Timeout.GetValueOrDefault();
                        DateTime v = new DateTime(to.Year, to.Month, to.Day, 18, 0, 0, DateTimeKind.Utc);
                        double x = (to - v).TotalSeconds / 3600.0;
                        duration += x;
                    }
                });

            return duration;
        }

        public static async Task<double> GetTotalOvertimeEarnings(Dictionary<string, object> filters, double duration)
        {
            int year = (int)filters["year"];

            ISession se = NHibernateHelper.CurrentSession;

            Overtimerate o = await Task.Run(() =>
            {
                return se.QueryOver<Overtimerate>()
                    .Where(x => x.Year == year).Skip(0).Take(1).SingleOrDefault();
            });

            double total = 0;

            if (o != null)
                total = (duration / o.Duration) * o.Payrate;

            return total;
        }
    }
}