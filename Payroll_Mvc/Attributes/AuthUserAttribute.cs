using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payroll_Mvc.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple=false)]
    public class AuthUserAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return IsNormalUser(httpContext.Session);
        }

        private bool IsNormalUser(HttpSessionStateBase se)
        {
            return se["employee_id"] != null;
        }
    }
}