using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

            ISession se = NHibernateHelper.CurrentSession;
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

            return l;
        }

        public static ListModel<User> GetFilterBy(Dictionary<string, object> filters, int pagenum = 1,
            int pagesize = Pager.DEFAULT_PAGE_SIZE, Sort sort = null)
        {
            ListModel<User> l = new ListModel<User>();

            if (sort == null)
                sort = new Sort(DEFAULT_SORT_COLUMN, DEFAULT_SORT_DIR);

            ISession se = NHibernateHelper.CurrentSession;
            ICriteria cr = se.CreateCriteria<User>("user");
            GetFilterCriteria(cr, filters);

            ICriteria crCount = se.CreateCriteria<User>("user");
            GetFilterCriteria(crCount, filters);

            int total = crCount.SetProjection(Projections.Count(Projections.Id())).UniqueResult<int>();
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

            return l;
        }

        public static User GetObject(FormCollection fc)
        {
            string paramStatus = fc.Get("status");
            bool status = paramStatus == "1" ? true : false;

            string paramRole = fc.Get("role");
            int role = Convert.ToInt32(paramRole);

            User o = new User
            {
                Role = role,
                Username = fc.Get("username"),
                Status = status,
                Password = fc.Get("pwd"),
                PasswordConfirmation = fc.Get("pwdconfirm")
            };

            return o;
        }

        public static string GetItemMessage(Dictionary<string, object> filters, int pagenum, int pagesize)
        {
            int total = 0;
            Pager pager = null;
            string m = null;
            ISession se = NHibernateHelper.CurrentSession;

            if (filters == null)
            {
                total = se.QueryOver<User>().Future().Count();
                pager = new Pager(total, pagenum, pagesize);
                m = pager.GetItemMessage();
            }

            else
            {
                ICriteria cr = se.CreateCriteria<User>("user");
                GetFilterCriteria(cr, filters);
                total = cr.SetProjection(Projections.Count(Projections.Id())).UniqueResult<int>();
                pager = new Pager(total, pagenum, pagesize);
                m = pager.GetItemMessage();
            }

            return m;
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