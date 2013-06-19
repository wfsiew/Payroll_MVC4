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
using Payroll_Mvc.Attributes;

namespace Payroll_Mvc.Areas.User.Controllers
{
    [Authorize]
    [AuthUser]
    public class PayslipController : AsyncController
    {
        //
        // GET: /User/Payslip/

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Payslip(int month, int year)
        {
            ISession se = NHibernateHelper.CurrentSession;

            object id = Session["employee_id"];
            Employee employee = se.Get<Employee>(id);
            Employeesalary employee_salary = employee.Employeesalary;

            PayslipModel o = new PayslipModel();
            o.Period = string.Format("{0}-{1}", CommonHelper.GetMonthName(month), year);
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

                    o.TotalEarnings = PayslipHelper.GetTotalEarnings(employee_salary, o.Adjustment, o.TotalOvertimeEarnings);
                    o.TotalDeduct = PayslipHelper.GetTotalDeductions(employee_salary);
                    o.NettSalary = PayslipHelper.GetNettSalary(o.TotalEarnings, o.TotalDeduct);

                    o.BasicPay = employee_salary.Salary + o.Adjustment;

                    return View("payslip_monthly", o);
                }

                else
                {
                    Dictionary<string, object> filters = new Dictionary<string, object>
                    {
                        { "year", year },
                        { "month", month },
                        { "staff_id", employee.Staffid }
                    };

                    double[] x = await PayslipHelper.GetTotalEarningsHourly(employee_salary, filters);
                    o.TotalEarnings = x[0];
                    o.TotalHours = x[1];
                    o.HourlyPayRate = x[2];

                    o.TotalDeduct = PayslipHelper.GetTotalDeductions(employee_salary);
                    o.NettSalary = PayslipHelper.GetNettSalaryHourly(o.TotalEarnings, o.TotalDeduct);

                    return View("payslip_hourly", o);
                }
            }
        }
    }
}
