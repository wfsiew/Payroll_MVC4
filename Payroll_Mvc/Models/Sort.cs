using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Payroll_Mvc.Models
{
    public class Sort
    {
        public string Column { get; set; }
        public string Direction { get; set; }

        public Sort(string column, string dir = "ASC")
        {
            Column = column;
            Direction = dir;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Column, Direction);
        }
    }
}