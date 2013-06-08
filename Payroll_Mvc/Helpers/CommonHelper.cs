using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Payroll_Mvc.Helpers
{
    public class CommonHelper
    {
        public static string FormatDate(DateTime? dt)
        {
            if (dt != null && dt.GetValueOrDefault() != DateTime.MinValue)
                return string.Format("{0:dd-MM-yyyy}", dt);

            return null;
        }
    }
}