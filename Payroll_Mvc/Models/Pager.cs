using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Payroll_Mvc.Models
{
    public class Pager
    {
        public int Total { get; set; }
        public int PageNum { get; set; }
        private int pageSize;

        public const int DEFAULT_PAGE_SIZE = 50;

        public Pager(int total, int pagenum, int pagesize)
        {
            Total = total;
            PageNum = pagenum;
            SetPageSize(pagesize);
        }

        public int PageSize
        {
            get
            {
                return pageSize;
            }

            set
            {
                SetPageSize(value);
            }
        }

        public string GetItemMessage()
        {
            return Utils.GetItemMessage(Total, PageNum, PageSize);
        }

        public int LowerBound
        {
            get
            {
                return (PageNum - 1) * PageSize;
            }
        }

        public int UpperBound
        {
            get
            {
                int upperbound = PageNum * PageSize;
                if (Total < upperbound)
                    upperbound = Total;

                return upperbound;
            }
        }

        public bool HasNext
        {
            get
            {
                return Total > UpperBound ? true : false;
            }
        }

        public bool HasPrev
        {
            get
            {
                return LowerBound > 0 ? true : false;
            }
        }

        public int TotalPages
        {
            get
            {
                return (int)(Math.Ceiling((double)Total / PageSize));
            }
        }

        private void SetPageSize(int pagesize)
        {
            if ((Total < pagesize || pagesize < 1) && Total > 0)
                pageSize = Total;

            else
                pageSize = pagesize;

            if (TotalPages < PageNum)
                PageNum = TotalPages;

            if (PageNum < 1)
                PageNum = 1;
        }
    }
}