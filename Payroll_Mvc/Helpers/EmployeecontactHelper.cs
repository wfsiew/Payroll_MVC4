using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Domain.Model;
using Payroll_Mvc.Models;

namespace Payroll_Mvc.Helpers
{
    public class EmployeecontactHelper
    {
        public static Employeecontact GetObject(Employee e, FormCollection fc)
        {
            Employeecontact o = e.Employeecontact;

            if (o == null)
            {
                o = new Employeecontact();
                o.Id = e.Id;
            }

            o.Address1 = GetParam("address_1", fc);
            o.Address2 = GetParam("address_2", fc);
            o.Address3 = GetParam("address_3", fc);
            o.City = GetParam("city", fc);
            o.State = GetParam("state", fc);
            o.Postcode = GetParam("postcode", fc);
            o.Country = GetParam("country", fc);
            o.Homephone = GetParam("home_phone", fc);
            o.Mobilephone = GetParam("mobile_phone", fc);
            o.Workemail = GetParam("work_email", fc);
            o.Otheremail = GetParam("other_email", fc);

            return o;
        }

        public static bool IsEmptyParams(FormCollection fc)
        {
            if (string.IsNullOrEmpty(GetParam("address_1", fc)) && string.IsNullOrEmpty(GetParam("address_2", fc)) &&
                string.IsNullOrEmpty(GetParam("address_3", fc)) && string.IsNullOrEmpty(GetParam("city", fc)) &&
                string.IsNullOrEmpty(GetParam("state", fc)) && string.IsNullOrEmpty(GetParam("postcode", fc)) &&
                string.IsNullOrEmpty(GetParam("country", fc)) && string.IsNullOrEmpty(GetParam("home_phone", fc)) &&
                string.IsNullOrEmpty(GetParam("mobile_phone", fc)) && string.IsNullOrEmpty(GetParam("work_email", fc)) &&
                string.IsNullOrEmpty(GetParam("other_email", fc)))
                return true;

            return false;
        }

        private static string GetParam(string key, FormCollection fc)
        {
            return fc.Get(string.Format("employee_contact[{0}]", key));
        }
    }
}