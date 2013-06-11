using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Domain.Helper
{
    public class CommonHelper
    {
        public const string DATE_FMT = "dd-MM-yyyy";

        public static DateTime GetDateTime(string q)
        {
            return string.IsNullOrEmpty(q) ? default(DateTime) : DateTime.ParseExact(q, DATE_FMT, CultureInfo.InvariantCulture);
        }
    }
}
