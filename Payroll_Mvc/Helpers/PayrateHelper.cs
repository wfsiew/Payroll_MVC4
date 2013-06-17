using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

using NHibernate;
using NHibernate.SqlCommand;
using NHibernate.Criterion;
using NHibernate.Criterion.Lambda;
using Domain.Model;
using Payroll_Mvc.Models;
using Payroll_Mvc.Areas.Admin.Models;

namespace Payroll_Mvc.Helpers
{
    public class PayrateHelper
    {
        public const string DEFAULT_SORT_COLUMN = "Staffid";
        public const string DEFAULT_SORT_DIR = "ASC";

        public static async Task<ListModel<Payrate>> GetAll(int pagenum = 1, int pagesize = Pager.DEFAULT_PAGE_SIZE,
            Sort sort = null)
        {
            ListModel<Payrate> l = new ListModel<Payrate>();

            if (sort == null)
                sort = new Sort(DEFAULT_SORT_COLUMN, DEFAULT_SORT_DIR);

            ISession se = NHibernateHelper.CurrentSession;
            int total = await Task.Run(() => { return se.QueryOver<Payrate>().Future().Count(); });
            Pager pager = new Pager(total, pagenum, pagesize);

            int has_next = pager.HasNext ? 1 : 0;
            int has_prev = pager.HasPrev ? 1 : 0;

            ICriteria cr = se.CreateCriteria<Payrate>("pr");
            GetOrder(sort, cr);

            cr.SetFirstResult(pager.LowerBound);
            cr.SetMaxResults(pager.PageSize);

            IList<Payrate> list = await Task.Run(() => { return cr.List<Payrate>(); });

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

        public static async Task<ListModel<Payrate>> GetFilterBy(Dictionary<string, object> filters, int pagenum = 1,
            int pagesize = Pager.DEFAULT_PAGE_SIZE, Sort sort = null)
        {
            ListModel<Payrate> l = new ListModel<Payrate>();

            if (sort == null)
                sort = new Sort(DEFAULT_SORT_COLUMN, DEFAULT_SORT_DIR);

            ISession se = NHibernateHelper.CurrentSession;
            ICriteria cr = se.CreateCriteria<Payrate>("pr");
            GetFilterCriteria(cr, filters);

            ICriteria crCount = se.CreateCriteria<Payrate>("pr");
            GetFilterCriteria(crCount, filters);

            int total = await Task.Run(() => { return crCount.SetProjection(Projections.Count(Projections.Id())).UniqueResult<int>(); });
            Pager pager = new Pager(total, pagenum, pagesize);

            int has_next = pager.HasNext ? 1 : 0;
            int has_prev = pager.HasPrev ? 1 : 0;

            GetOrder(sort, cr);

            cr.SetFirstResult(pager.LowerBound);
            cr.SetMaxResults(pager.PageSize);

            IList<Payrate> list = await Task.Run(() => { return cr.List<Payrate>(); });

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

        public static Payrate GetObject(Payrate o, FormCollection fc)
        {
            string staff_id = fc.Get("staff_id");

            string paramMonth = fc.Get("month");
            int month = CommonHelper.GetValue<int>(paramMonth);

            string paramYear = fc.Get("year");
            int year = CommonHelper.GetValue<int>(paramYear);

            string paramPayRate = fc.Get("pay_rate");
            double hourlyPayRate = CommonHelper.GetValue<double>(paramPayRate);

            if (o == null)
                o = new Payrate();

            o.Staffid = staff_id;
            o.Month = month;
            o.Year = year;
            o.Hourlypayrate = hourlyPayRate;

            return o;
        }

        public static async Task<string> GetItemMessage(Dictionary<string, object> filters, int pagenum, int pagesize)
        {
            int total = 0;
            Pager pager = null;
            string m = null;
            ISession se = NHibernateHelper.CurrentSession;

            if (filters == null)
            {
                total = await Task.Run(() => { return se.QueryOver<Payrate>().Future().Count(); });
                pager = new Pager(total, pagenum, pagesize);
                m = pager.GetItemMessage();
            }

            else
            {
                ICriteria cr = se.CreateCriteria<Payrate>("pr");
                GetFilterCriteria(cr, filters);
                total = await Task.Run(() => { return cr.SetProjection(Projections.Count(Projections.Id())).UniqueResult<int>(); });
                pager = new Pager(total, pagenum, pagesize);
                m = pager.GetItemMessage();
            }

            return m;
        }

        public static async Task<double> GetPayRate(Dictionary<string, object> filters)
        {
            ISession se = NHibernateHelper.CurrentSession;
            ICriteria cr = se.CreateCriteria<Payrate>();

            cr.Add(Restrictions.Eq("Staffid", filters["staff_id"]));
            cr.Add(Restrictions.Eq("Year", filters["year"]));
            cr.Add(Restrictions.Eq("Month", filters["month"]));

            Payrate o = await Task.Run(() => { return cr.List<Payrate>().SingleOrDefault(); });
            double rate = 0;

            if (o != null)
                rate = o.Hourlypayrate;

            return rate;
        }

        private static void GetOrder(Sort sort, ICriteria cr)
        {
            bool sortDir = sort.Direction == "ASC" ? true : false;
            Order order = new Order(string.Format("pr.{0}", sort.Column), sortDir);
            cr.AddOrder(order);
        }

        private static void GetFilterCriteria(ICriteria cr, Dictionary<string, object> filters)
        {
            string staff_id = Convert.ToString(filters["staff_id"]);
            int month = (int)filters["month"];
            int year = (int)filters["year"];

            if (!string.IsNullOrEmpty(staff_id))
                cr.Add(Restrictions.InsensitiveLike("pr.Staffid", staff_id, MatchMode.Anywhere));

            if (month != 0)
                cr.Add(Restrictions.Eq("pr.Month", month));

            if (year != 0)
                cr.Add(Restrictions.Eq("pr.Year", year));
        }
    }
}