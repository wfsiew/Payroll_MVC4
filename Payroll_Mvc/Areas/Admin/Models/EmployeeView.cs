using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Domain.Model;

namespace Payroll_Mvc.Areas.Admin.Models
{
    public class EmployeeView
    {
        public Employee Employee { get; set; }
        public Employeecontact Employeecontact { get; set; }
        public Employeejob Employeejob { get; set; }
        public Employeequalification Employeequalification { get; set; }
        public Employeesalary Employeesalary { get; set; }
    }
}