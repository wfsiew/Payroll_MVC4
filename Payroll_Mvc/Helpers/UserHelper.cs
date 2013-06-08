using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using NHibernate;
using NHibernate.SqlCommand;
using NHibernate.Criterion;
using NHibernate.Criterion.Lambda;
using Domain.Model;
using Payroll_Mvc.Models;

namespace Payroll_Mvc.Helpers
{
    public class UserHelper
    {
        public const string DEFAULT_SORT_COLUMN = "Username";
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

                int has_next = pager.HasNext ? 1 : 0;
                int has_prev = pager.HasPrev ? 1 : 0;

                ICriteria cr = se.CreateCriteria<User>("user");
                GetOrder(sort, cr);

                cr.SetFirstResult(pager.LowerBound);
                cr.SetMaxResults(pager.PageSize);

                List<User> list = cr.List<User>().ToList();

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

        public static ListModel<User> GetFilterBy(Dictionary<string, object> filters, int pagenum = 1,
            int pagesize = Pager.DEFAULT_PAGE_SIZE, Sort sort = null)
        {
            ListModel<User> l = new ListModel<User>();

            if (sort == null)
                sort = new Sort(DEFAULT_SORT_COLUMN, DEFAULT_SORT_DIR);

            using (ISession se = NHibernateHelper.OpenSession())
            {
                ICriteria cr = se.CreateCriteria<User>("user");
                GetFilterCriteria(cr, filters);

                int total = cr.List().Count;
                Pager pager = new Pager(total, pagenum, pagesize);

                int has_next = pager.HasNext ? 1 : 0;
                int has_prev = pager.HasPrev ? 1 : 0;

                GetOrder(sort, cr);

                cr.SetFirstResult(pager.LowerBound);
                cr.SetMaxResults(pager.PageSize);

                List<User> list = cr.List<User>().ToList();

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

        private static void GetOrder(Sort sort, ICriteria cr)
        {
            bool sortDir = sort.Direction == "ASC" ? true : false;
            Order order = new Order(string.Format("user.{0}", sort.Column), sortDir);
            cr.AddOrder(order);
        }

        private static void GetFilterCriteria(ICriteria cr, Dictionary<string, object> filters)
        {
            string employee = Convert.ToString(filters["employee"]);
            string username = Convert.ToString(filters["username"]);
            int role = (int)filters["role"];
            int status = (int)filters["status"];
            bool statusVal = status == 1 ? true : false;

            if (!string.IsNullOrEmpty(username))
                cr.Add(Restrictions.InsensitiveLike("user.Username", username, MatchMode.Anywhere));

            if (role != 0)
                cr.Add(Restrictions.Eq("user.Role", role));

            if (status != 0)
                cr.Add(Restrictions.Eq("user.Status", statusVal));

            if (!string.IsNullOrEmpty(employee))
            {
                cr = cr.CreateCriteria("user.Employee", "e", JoinType.InnerJoin);
                AbstractCriterion a1 = Restrictions.InsensitiveLike("e.Firstname", employee, MatchMode.Anywhere);
                AbstractCriterion a2 = Restrictions.InsensitiveLike("e.Middlename", employee, MatchMode.Anywhere);
                AbstractCriterion a3 = Restrictions.InsensitiveLike("e.Lastname", employee, MatchMode.Anywhere);

                AbstractCriterion b1 = Restrictions.Or(a1, a2);
                AbstractCriterion b2 = Restrictions.Or(b1, a3);

                cr.Add(b2);
            }
        }
    }
}