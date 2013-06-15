using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Payroll_Mvc.Helpers
{
    public class CommonHelper
    {
        public const string DATE_FMT = "dd-MM-yyyy";
        public const int END_YEAR = 2000;

        public static string FormatDate(DateTime? dt)
        {
            if (dt != null && dt.GetValueOrDefault() != default(DateTime))
                return string.Format("{0:dd-MM-yyyy}", dt);

            return null;
        }

        public static string FormatTime(DateTime? dt)
        {
            if (dt != null && dt.GetValueOrDefault() != default(DateTime))
                return string.Format("{0:h:mm tt}", dt);

            return null;
        }

        public static DateTime GetDateTime(string q)
        {
            return string.IsNullOrEmpty(q) ? default(DateTime) : DateTime.ParseExact(q, DATE_FMT, CultureInfo.InvariantCulture);
        }

        public static string FormatNumberInt(int x)
        {
            if (x == default(int))
                return null;

            return Convert.ToString(x);
        }

        public static string FormatNumberDouble(double x)
        {
            if (x == default(double))
                return null;

            return Convert.ToString(x);
        }

        public static string FormatNumber(double x)
        {
            if (x == default(double))
                return null;

            return string.Format("{0:N2}", x);
        }

        public static Dictionary<string, object> GetErrors(Dictionary<string, object> o)
        {
            if (o == null)
                return new Dictionary<string, object>();

            return o;
        }

        public static string GetAbbreviatedMonthName(int i)
        {
            return DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(i);
        }

        public static string GetMonthName(int i)
        {
            return DateTimeFormatInfo.CurrentInfo.GetMonthName(i);
        }

        public static T GetValue<T>(string p, T k = default(T)) where T : struct
        {
            T x = k;

            if (string.IsNullOrEmpty(p))
                return x;

            try
            {
                object o = Convert.ChangeType(p, typeof(T));
                x = (T)o;
            }

            catch
            {
                x = k;
            }

            return x;
        }

        public static string GetValue(string p, string k = "")
        {
            if (string.IsNullOrEmpty(p))
                return k;

            return p;
        }
    }
}