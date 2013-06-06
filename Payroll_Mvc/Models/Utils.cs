using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Payroll_Mvc.Models
{
    public static class Utils
    {
        public static string GetItemMessage(int total, int pagenum, int pagesize)
        {
            int x = (pagenum - 1) * pagesize + 1;
            int y = pagenum * pagesize;

            if (total < y)
                y = total;

            if (total < 1)
                return "";

            return string.Format("{0} to {1} of {2}", x, y, total);
        }

        public static bool IsNumeric(object val)
        {
            bool a = false;

            try
            {
                decimal x = Convert.ToDecimal(val);
                a = true;
            }

            catch
            {
            }

            return a;
        }

        public static string MonthName(int m)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m);
        }

        public static DateTime LocalTime(DateTime t)
        {
            TimeZoneInfo ti = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(t, ti);
        }
    }
}