using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Domain.Model;

namespace Payroll_Mvc.Models
{
    public class PayslipModel
    {
        public Employee Employee { get; set; }
        public Employeesalary EmployeeSalary { get; set; }
        public string Period { get; set; }
        public double TotalEarnings { get; set; }
        public double TotalDeduct { get; set; }
        public double NettSalary { get; set; }
        public double TotalOvertime { get; set; }
        public double TotalOvertimeEarnings { get; set; }
        public double Adjustment { get; set; }
        public double BasicPay { get; set; }
        public double TotalHours { get; set; }
        public double HourlyPayRate { get; set; }
    }
}