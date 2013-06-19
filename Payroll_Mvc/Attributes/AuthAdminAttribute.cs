using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payroll_Mvc.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class AuthAdminAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return IsAdminUser(httpContext.Session);
        }

        private bool IsAdminUser(HttpSessionStateBase se)
        {
            return se["admin_id"] != null;
        }
    }
}