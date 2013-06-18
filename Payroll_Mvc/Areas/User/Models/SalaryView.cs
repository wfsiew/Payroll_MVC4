using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Domain.Model;

namespace Payroll_Mvc.Areas.User.Models
{
    public class SalaryView
    {
        public Employeesalary Employeesalary { get; set; }
        public double BasicPay { get; set; }
    }
}