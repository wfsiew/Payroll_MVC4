using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using NHibernate;
using NHibernate.Criterion.Lambda;
using Domain.Model;
using Payroll_Mvc.Models;

namespace Payroll_Mvc.Helpers
{
    public class UserHelper
    {
        public const string DEFAULT_SORT_COLUMN = "username";
        public const string DEFAULT_SORT_DIR = "ASC";

        public static ListModel<User> GetAll(int pagenum = 1, int pagesize = Pager.DEFAULT_PAGE_SIZE,
            Sort sort = null)
        {
            ListModel<User> l = new ListModel<User>();

            if (sort == null)
                sort = new Sort(DEFAULT_SORT_COLUMN, DEFAULT_SORT_DIR);

            using (ISession se = NHibernateHelper.OpenSession())
            {
                int total = se.QueryOver<User>().Future().Count();
                Pager pager = new Pager(total, pagenum, pagesize);
                string order = sort.ToString();

                int has_next = pager.HasNext ? 1 : 0;
                int has_prev = pager.HasPrev ? 1 : 0;

                IQueryOver<User, User> q = se.QueryOver<User>();
                q = GetOrder(sort, q);
                List<User> list = q.Take(pager.PageSize).Skip(pager.LowerBound).List().ToList();

                l.ItemMsg = pager.GetItemMessage();
                l.HasNext = has_next;
                l.HasPrev = has_prev;
                l.NextPage = pager.PageNum + 1;
                l.PrevPage = pager.PageNum - 1;
                l.List = list;
                l.SortColumn = sort.Column;
                l.SortDir = sort.Direction;
                l.Page = pager.PageNum;
                l.TotalPage = pager.TotalPages;
            }

            return l;
        }

        private static IQueryOver<User, User> GetOrder(Sort sort, IQueryOver<User, User> q)
        {
            IQueryOverOrderBuilder<User, User> o = q.OrderBy(x => x.Username);
            IQueryOver<User, User> v = null;

            if (sort.Column == "role")
                o = q.OrderBy(x => x.Role);

            else if (sort.Column == "status")
                o = q.OrderBy(x => x.Status);

            if (sort.Direction == "ASC")
                v = o.Asc;

            else
                v = o.Desc;

            return v;
        }
    }
}