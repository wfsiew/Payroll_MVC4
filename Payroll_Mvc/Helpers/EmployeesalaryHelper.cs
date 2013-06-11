using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Domain.Model;
using Payroll_Mvc.Models;

namespace Payroll_Mvc.Helpers
{
    public class EmployeesalaryHelper
    {
        public static Employeesalary GetObject(Employee e,FormCollection fc)
        {
            string paramSalary = GetParam("salary", fc);
            double salary = string.IsNullOrEmpty(paramSalary) ? default(double) : Convert.ToDouble(paramSalary);

            string paramAllowance = GetParam("allowance", fc);
            double allowance = string.IsNullOrEmpty(paramAllowance) ? default(double) : Convert.ToDouble(paramAllowance);

            string paramEpf = GetParam("epf", fc);
            double epf = string.IsNullOrEmpty(paramEpf) ? default(double) : Convert.ToDouble(paramEpf);

            string paramSocso = GetParam("socso", fc);
            double socso = string.IsNullOrEmpty(paramSocso) ? default(double) : Convert.ToDouble(paramSocso);

            string paramIncometax = GetParam("income_tax", fc);
            double incometax = string.IsNullOrEmpty(paramIncometax) ? default(double) : Convert.ToDouble(paramIncometax);

            string paramPaytype = GetParam("pay_type", fc);
            int? paytype = string.IsNullOrEmpty(paramPaytype) ? null : new Nullable<int>(Convert.ToInt32(paramPaytype));

            Employeesalary o = e.Employeesalary;

            if (o == null)
            {
                o = new Employeesalary();
                o.Id = e.Id;
            }

            o.Salary = salary;
            o.Allowance = allowance;
            o.Epf = epf;
            o.Socso = socso;
            o.Incometax = incometax;
            o.Bankname = GetParam("bank_name", fc);
            o.Bankaccno = GetParam("bank_acc_no", fc);
            o.Bankacctype = GetParam("bank_acc_type", fc);
            o.Bankaddress = GetParam("bank_address", fc);
            o.Epfno = GetParam("epf_no", fc);
            o.Socsono = GetParam("socso_no", fc);
            o.Incometaxno = GetParam("ncome_tax_no", fc);
            o.Paytype = paytype;

            return o;
        }

        public static bool IsEmptyParams(FormCollection fc)
        {
            if (string.IsNullOrEmpty(GetParam("salary", fc)) && string.IsNullOrEmpty(GetParam("allowance", fc)) &&
                string.IsNullOrEmpty(GetParam("epf", fc)) && string.IsNullOrEmpty(GetParam("socso", fc)) &&
                string.IsNullOrEmpty(GetParam("bank_name", fc)) && string.IsNullOrEmpty(GetParam("bank_acc_no", fc)) &&
                string.IsNullOrEmpty(GetParam("bank_acc_type", fc)) && string.IsNullOrEmpty(GetParam("bank_address", fc)) &&
                string.IsNullOrEmpty(GetParam("epf_no", fc)) && string.IsNullOrEmpty(GetParam("socso_no", fc)) &&
                string.IsNullOrEmpty(GetParam("income_tax_no", fc)))
                return true;

            else if (GetParam("salary", fc) == "0" && GetParam("allowance", fc) == "0" && GetParam("epf", fc) == "0" &&
                GetParam("socso", fc) == "0" && string.IsNullOrEmpty(GetParam("bank_name", fc)) &&
                string.IsNullOrEmpty(GetParam("bank_acc_no", fc)) && string.IsNullOrEmpty(GetParam("bank_acc_type", fc)) &&
                string.IsNullOrEmpty(GetParam("bank_address", fc)) && string.IsNullOrEmpty(GetParam("epf_no", fc)) &&
                string.IsNullOrEmpty(GetParam("socso_no", fc)) && string.IsNullOrEmpty(GetParam("income_tax_no", fc)))
                return true;

            return false;
        }

        private static string GetParam(string key, FormCollection fc)
        {
            return fc.Get(string.Format("employee_salary[{0}]", key));
        }
    }
}