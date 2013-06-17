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

namespace Payroll_Mvc.Helpers
{
    public class JobcategoryHelper
    {
        public const string DEFAULT_SORT_COLUMN = "Name";
        public const string DEFAULT_SORT_DIR = "ASC";

        public static async Task<ListModel<Jobcategory>> GetAll(int pagenum = 1, int pagesize = Pager.DEFAULT_PAGE_SIZE,
            Sort sort = null)
        {
            ListModel<Jobcategory> l = new ListModel<Jobcategory>();

            if (sort == null)
                sort = new Sort(DEFAULT_SORT_COLUMN, DEFAULT_SORT_DIR);

            ISession se = NHibernateHelper.CurrentSession;
            int total = await Task.Run(() => { return se.QueryOver<Jobcategory>().Future().Count(); });
            Pager pager = new Pager(total, pagenum, pagesize);

            int has_next = pager.HasNext ? 1 : 0;
            int has_prev = pager.HasPrev ? 1 : 0;

            ICriteria cr = se.CreateCriteria<Jobcategory>("jobcat");
            GetOrder(sort, cr);

            cr.SetFirstResult(pager.LowerBound);
            cr.SetMaxResults(pager.PageSize);

            IList<Jobcategory> list = await Task.Run(() => { return cr.List<Jobcategory>(); });

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

        public static async Task<ListModel<Jobcategory>> GetFilterBy(string keyword, int pagenum = 1,
            int pagesize = Pager.DEFAULT_PAGE_SIZE, Sort sort = null)
        {
            ListModel<Jobcategory> l = new ListModel<Jobcategory>();

            if (sort == null)
                sort = new Sort(DEFAULT_SORT_COLUMN, DEFAULT_SORT_DIR);

            ISession se = NHibernateHelper.CurrentSession;
            ICriteria cr = se.CreateCriteria<Jobcategory>("jobcat");
            GetFilterCriteria(cr, keyword);

            ICriteria crCount = se.CreateCriteria<Jobcategory>("jobcat");
            GetFilterCriteria(crCount, keyword);

            int total = await Task.Run(() => { return crCount.SetProjection(Projections.Count(Projections.Id())).UniqueResult<int>(); });
            Pager pager = new Pager(total, pagenum, pagesize);

            int has_next = pager.HasNext ? 1 : 0;
            int has_prev = pager.HasPrev ? 1 : 0;

            GetOrder(sort, cr);

            cr.SetFirstResult(pager.LowerBound);
            cr.SetMaxResults(pager.PageSize);

            IList<Jobcategory> list = await Task.Run(() => { return cr.List<Jobcategory>(); });

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

        public static Jobcategory GetObject(Jobcategory o, FormCollection fc)
        {
            if (o == null)
                o = new Jobcategory();

            o.Name = fc.Get("name");

            return o;
        }

        public static async Task<string> GetItemMessage(string keyword, int pagenum, int pagesize)
        {
            int total = 0;
            Pager pager = null;
            string m = null;
            ISession se = NHibernateHelper.CurrentSession;

            if (string.IsNullOrEmpty(keyword))
            {
                total = await Task.Run(() => { return se.QueryOver<Jobcategory>().Future().Count(); });
                pager = new Pager(total, pagenum, pagesize);
                m = pager.GetItemMessage();
            }

            else
            {
                ICriteria cr = se.CreateCriteria<Jobcategory>("jobcat");
                GetFilterCriteria(cr, keyword);
                total = await Task.Run(() => { return cr.SetProjection(Projections.Count(Projections.Id())).UniqueResult<int>(); });
                pager = new Pager(total, pagenum, pagesize);
                m = pager.GetItemMessage();
            }

            return m;
        }

        private static void GetOrder(Sort sort, ICriteria cr)
        {
            bool sortDir = sort.Direction == "ASC" ? true : false;
            Order order = new Order(string.Format("jobcat.{0}", sort.Column), sortDir);
            cr.AddOrder(order);
        }

        private static void GetFilterCriteria(ICriteria cr, string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                cr.Add(Restrictions.InsensitiveLike("jobcat.Name", keyword, MatchMode.Anywhere));
        }
    }
}