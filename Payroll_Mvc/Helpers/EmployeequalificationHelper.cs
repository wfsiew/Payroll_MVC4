using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Domain.Model;
using Payroll_Mvc.Models;

namespace Payroll_Mvc.Helpers
{
    public class EmployeequalificationHelper
    {
        public static Employeequalification GetObject(Employee e, FormCollection fc)
        {
            string paramStartdate = GetParam("start_date", fc);
            DateTime startdate = CommonHelper.GetDateTime(paramStartdate);

            string paramEnddate = GetParam("end_date", fc);
            DateTime enddate = CommonHelper.GetDateTime(paramEnddate);

            string paramLevel = GetParam("level", fc);
            int level = CommonHelper.GetValue<int>(paramLevel);

            string paramYear = GetParam("year", fc);
            int year = CommonHelper.GetValue<int>(paramYear);

            string paramGpa = GetParam("gpa", fc);
            double gpa = CommonHelper.GetValue<double>(paramGpa);

            Employeequalification o = e.Employeequalification;

            if (o == null)
            {
                o = new Employeequalification();
                o.Id = e.Id;
            }

            o.Level = level;
            o.Institute = GetParam("institute", fc);
            o.Major = GetParam("major", fc);
            o.Year = year;
            o.Gpa = gpa;
            o.Startdate = startdate;
            o.Enddate = enddate;

            return o;
        }

        public static bool IsEmptyParams(FormCollection fc)
        {
            if (GetParam("level", fc) == "0" && string.IsNullOrEmpty(GetParam("institute", fc)) &&
                string.IsNullOrEmpty(GetParam("major", fc)) && string.IsNullOrEmpty(GetParam("year", fc)) &&
                string.IsNullOrEmpty(GetParam("gpa", fc)) && string.IsNullOrEmpty(GetParam("start_date", fc)) &&
                string.IsNullOrEmpty(GetParam("end_date", fc)))
                return true;

            return false;
        }

        private static string GetParam(string key, FormCollection fc)
        {
            return fc.Get(string.Format("employee_qualification[{0}]", key));
        }
    }
}