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
    public class OvertimerateHelper
    {
        public const string DEFAULT_SORT_COLUMN = "Year";
        public const string DEFAULT_SORT_DIR = "ASC";

        public static async Task<ListModel<Overtimerate>> GetAll(int pagenum = 1, int pagesize = Pager.DEFAULT_PAGE_SIZE,
            Sort sort = null)
        {
            ListModel<Overtimerate> l = new ListModel<Overtimerate>();

            if (sort == null)
                sort = new Sort(DEFAULT_SORT_COLUMN, DEFAULT_SORT_DIR);

            ISession se = NHibernateHelper.CurrentSession;
            int total = await Task.Run(() => { return se.QueryOver<Overtimerate>().Future().Count(); });
            Pager pager = new Pager(total, pagenum, pagesize);

            int has_next = pager.HasNext ? 1 : 0;
            int has_prev = pager.HasPrev ? 1 : 0;

            ICriteria cr = se.CreateCriteria<Overtimerate>("ov");
            GetOrder(sort, cr);

            cr.SetFirstResult(pager.LowerBound);
            cr.SetMaxResults(pager.PageSize);

            IList<Overtimerate> list = await Task.Run(() => { return cr.List<Overtimerate>(); });

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

        public static async Task<ListModel<Overtimerate>> GetFilterBy(Dictionary<string, object> filters, int pagenum = 1,
            int pagesize = Pager.DEFAULT_PAGE_SIZE, Sort sort = null)
        {
            ListModel<Overtimerate> l = new ListModel<Overtimerate>();

            if (sort == null)
                sort = new Sort(DEFAULT_SORT_COLUMN, DEFAULT_SORT_DIR);

            ISession se = NHibernateHelper.CurrentSession;
            ICriteria cr = se.CreateCriteria<Overtimerate>("ov");
            GetFilterCriteria(cr, filters);

            ICriteria crCount = se.CreateCriteria<Overtimerate>("ov");
            GetFilterCriteria(crCount, filters);

            int total = await Task.Run(() => { return crCount.SetProjection(Projections.Count(Projections.Id())).UniqueResult<int>(); });
            Pager pager = new Pager(total, pagenum, pagesize);

            int has_next = pager.HasNext ? 1 : 0;
            int has_prev = pager.HasPrev ? 1 : 0;

            GetOrder(sort, cr);

            cr.SetFirstResult(pager.LowerBound);
            cr.SetMaxResults(pager.PageSize);

            IList<Overtimerate> list = await Task.Run(() => { return cr.List<Overtimerate>(); });

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

        public static Overtimerate GetObject(Overtimerate o, FormCollection fc)
        {
            string paramDuration = fc.Get("duration");
            double duration = CommonHelper.GetValue<double>(paramDuration);

            string paramYear = fc.Get("year");
            int year = CommonHelper.GetValue<int>(paramYear);

            string paramPayRate = fc.Get("pay_rate");
            double payRate = CommonHelper.GetValue<double>(paramPayRate);

            if (o == null)
                o = new Overtimerate();

            o.Duration = duration;
            o.Year = year;
            o.Payrate = payRate;

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
                total = await Task.Run(() => { return se.QueryOver<Overtimerate>().Future().Count(); });
                pager = new Pager(total, pagenum, pagesize);
                m = pager.GetItemMessage();
            }

            else
            {
                ICriteria cr = se.CreateCriteria<Overtimerate>("ov");
                GetFilterCriteria(cr, filters);
                total = await Task.Run(() => { return cr.SetProjection(Projections.Count(Projections.Id())).UniqueResult<int>(); });
                pager = new Pager(total, pagenum, pagesize);
                m = pager.GetItemMessage();
            }

            return m;
        }

        private static void GetOrder(Sort sort, ICriteria cr)
        {
            bool sortDir = sort.Direction == "ASC" ? true : false;
            Order order = new Order(string.Format("ov.{0}", sort.Column), sortDir);
            cr.AddOrder(order);
        }

        private static void GetFilterCriteria(ICriteria cr, Dictionary<string, object> filters)
        {
            int year = (int)filters["year"];

            if (year != 0)
                cr.Add(Restrictions.Eq("ov.Year", year));
        }
    }
}