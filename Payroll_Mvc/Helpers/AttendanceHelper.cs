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
using Payroll_Mvc.Areas.Admin.Models;

namespace Payroll_Mvc.Helpers
{
    public class AttendanceHelper
    {
        public const string DEFAULT_SORT_COLUMN = "Workdate";
        public const string DEFAULT_SORT_DIR = "ASC";

        public static ListModel<Attendance> GetAll(int pagenum = 1, int pagesize = Pager.DEFAULT_PAGE_SIZE,
            Sort sort = null)
        {
            ListModel<Attendance> l = new ListModel<Attendance>();

            if (sort == null)
                sort = new Sort(DEFAULT_SORT_COLUMN, DEFAULT_SORT_DIR);

            ISession se = NHibernateHelper.CurrentSession;
            int total = se.QueryOver<Attendance>().Future().Count();
            Pager pager = new Pager(total, pagenum, pagesize);

            int has_next = pager.HasNext ? 1 : 0;
            int has_prev = pager.HasPrev ? 1 : 0;

            ICriteria cr = se.CreateCriteria<Attendance>("attendance");

            if (sort.Column == "e.Firstname")
                SetJoinCriteria(cr, sort);

            GetOrder(sort, cr);

            cr.SetFirstResult(pager.LowerBound);
            cr.SetMaxResults(pager.PageSize);

            List<Attendance> list = cr.List<Attendance>().ToList();

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

        public static ListModel<Attendance> GetFilterBy(Dictionary<string, object> filters, int pagenum = 1,
            int pagesize = Pager.DEFAULT_PAGE_SIZE, Sort sort = null)
        {
            ListModel<Attendance> l = new ListModel<Attendance>();

            if (sort == null)
                sort = new Sort(DEFAULT_SORT_COLUMN, DEFAULT_SORT_DIR);

            ISession se = NHibernateHelper.CurrentSession;
            ICriteria cr = se.CreateCriteria<Attendance>("attendance");
            GetFilterCriteria(cr, filters, sort);

            ICriteria crCount = se.CreateCriteria<Attendance>("attendance");
            GetFilterCriteria(crCount, filters, sort);

            int total = crCount.SetProjection(Projections.Count(Projections.Id())).UniqueResult<int>();
            Pager pager = new Pager(total, pagenum, pagesize);

            int has_next = pager.HasNext ? 1 : 0;
            int has_prev = pager.HasPrev ? 1 : 0;

            GetOrder(sort, cr);

            cr.SetFirstResult(pager.LowerBound);
            cr.SetMaxResults(pager.PageSize);

            List<Attendance> list = cr.List<Attendance>().ToList();

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

        public static Employee GetEmployee(string staffid, Dictionary<string, Employee> o)
        {
            if (o.ContainsKey(staffid))
                return o[staffid];

            ISession se = NHibernateHelper.CurrentSession;
            Employee e = se.QueryOver<Employee>().Where(x => x.Staffid == staffid).Skip(0).Take(1).SingleOrDefault();
            o[staffid] = e;

            return e;
        }

        public static double GetTotalHours(Dictionary<string, object> filters)
        {
            ISession se = NHibernateHelper.CurrentSession;
            ICriteria cr = se.CreateCriteria<Attendance>();

            IProjection yearProjection = Projections.SqlFunction("year", NHibernateUtil.Int32, Projections.Property("Workdate"));
            cr.Add(Restrictions.Eq(yearProjection, filters["year"]));

            IProjection monthProjection = Projections.SqlFunction("month", NHibernateUtil.Int32, Projections.Property("Workdate"));
            cr.Add(Restrictions.Eq(monthProjection, filters["month"]));

            if (filters.ContainsKey("staff_id"))
                cr.Add(Restrictions.Eq("Staffid", filters["staff_id"]));

            IList<Attendance> list = cr.List<Attendance>();
            double total_hours = 0;

            foreach (Attendance o in list)
            {
                DateTime to = o.Timeout.GetValueOrDefault();
                DateTime ti = o.Timein.GetValueOrDefault();
                total_hours += (to - ti).TotalSeconds / 3600.0;
            }

            return total_hours;
        }

        private static void GetOrder(Sort sort, ICriteria cr)
        {
            bool sortDir = sort.Direction == "ASC" ? true : false;
            Order order = null;

            if (sort.Column.IndexOf(".", StringComparison.OrdinalIgnoreCase) >= 0)
                order = new Order(sort.Column, sortDir);

            else
                order = new Order(string.Format("attendance.{0}", sort.Column), sortDir);

            cr.AddOrder(order);
        }

        private static void GetFilterCriteria(ICriteria cr, Dictionary<string, object> filters, Sort sort = null)
        {
            string employee = Convert.ToString(filters["employee"]);
            DateTime work_date = (DateTime)filters["work_date"];

            if (!string.IsNullOrEmpty(employee))
            {
                cr = cr.CreateCriteria("attendance.Employee", "e", JoinType.InnerJoin);

                AbstractCriterion a1 = Restrictions.InsensitiveLike("e.Firstname", employee, MatchMode.Anywhere);
                AbstractCriterion a2 = Restrictions.InsensitiveLike("e.Middlename", employee, MatchMode.Anywhere);
                AbstractCriterion a3 = Restrictions.InsensitiveLike("e.Lastname", employee, MatchMode.Anywhere);

                AbstractCriterion b1 = Restrictions.Or(a1, a2);
                AbstractCriterion b2 = Restrictions.Or(b1, a3);

                cr.Add(b2);
            }

            if (work_date != default(DateTime))
                cr.Add(Restrictions.Eq("attendance.Workdate", work_date));

            if (sort != null)
                SetJoinCriteria(cr, sort);
        }

        private static void SetJoinCriteria(ICriteria cr, Sort sort)
        {
            if (sort.Column == "e.Firstname")
            {
                ICriteria cre = cr.GetCriteriaByAlias("e");
                if (cre == null)
                    cr = cr.CreateCriteria("attendance.Employee", "e", JoinType.LeftOuterJoin);
            }
        }
    }
}