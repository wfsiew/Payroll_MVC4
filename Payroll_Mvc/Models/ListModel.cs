using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Payroll_Mvc.Models
{
    public class ListModel<T>
    {
        public string ItemMsg { get; set; }
        public int HasNext { get; set; }
        public int HasPrev { get; set; }
        public int NextPage { get; set; }
        public int PrevPage { get; set; }
        public List<T> List { get; set; }
        public string SortColumn { get; set; }
        public string SortDir { get; set; }
        public int Page { get; set; }
        public int TotalPage { get; set; }
    }
}