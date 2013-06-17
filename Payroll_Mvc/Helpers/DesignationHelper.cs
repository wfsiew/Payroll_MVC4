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
    public class DesignationHelper
    {
        public const string DEFAULT_SORT_COLUMN = "Title";
        public const string DEFAULT_SORT_DIR = "ASC";

        public static async Task<ListModel<Designation>> GetAll(int pagenum = 1, int pagesize = Pager.DEFAULT_PAGE_SIZE,
            Sort sort = null)
        {
            ListModel<Designation> l = new ListModel<Designation>();

            if (sort == null)
                sort = new Sort(DEFAULT_SORT_COLUMN, DEFAULT_SORT_DIR);

            ISession se = NHibernateHelper.CurrentSession;
            int total = await Task.Run(() => { return se.QueryOver<Designation>().Future().Count(); });
            Pager pager = new Pager(total, pagenum, pagesize);

            int has_next = pager.HasNext ? 1 : 0;
            int has_prev = pager.HasPrev ? 1 : 0;

            ICriteria cr = se.CreateCriteria<Designation>("des");
            GetOrder(sort, cr);

            cr.SetFirstResult(pager.LowerBound);
            cr.SetMaxResults(pager.PageSize);

            IList<Designation> list = await Task.Run(() => { return cr.List<Designation>(); });

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

        public static async Task<ListModel<Designation>> GetFilterBy(int find, string keyword, int pagenum = 1,
            int pagesize = Pager.DEFAULT_PAGE_SIZE, Sort sort = null)
        {
            ListModel<Designation> l = new ListModel<Designation>();

            if (sort == null)
                sort = new Sort(DEFAULT_SORT_COLUMN, DEFAULT_SORT_DIR);

            ISession se = NHibernateHelper.CurrentSession;
            ICriteria cr = se.CreateCriteria<Designation>("des");
            GetFilterCriteria(cr, find, keyword);

            ICriteria crCount = se.CreateCriteria<Designation>("des");
            GetFilterCriteria(crCount, find, keyword);

            int total = await Task.Run(() => { return crCount.SetProjection(Projections.Count(Projections.Id())).UniqueResult<int>(); });
            Pager pager = new Pager(total, pagenum, pagesize);

            int has_next = pager.HasNext ? 1 : 0;
            int has_prev = pager.HasPrev ? 1 : 0;

            GetOrder(sort, cr);

            cr.SetFirstResult(pager.LowerBound);
            cr.SetMaxResults(pager.PageSize);

            IList<Designation> list = await Task.Run(() => { return cr.List<Designation>(); });

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

        public static Designation GetObject(Designation o, FormCollection fc)
        {
            if (o == null)
                o = new Designation();

            o.Title = fc.Get("title");
            o.Description = fc.Get("desc");
            o.Note = fc.Get("note");

            return o;
        }

        public static async Task<string> GetItemMessage(int find, string keyword, int pagenum, int pagesize)
        {
            int total = 0;
            Pager pager = null;
            string m = null;
            ISession se = NHibernateHelper.CurrentSession;

            if (find == 0 && string.IsNullOrEmpty(keyword))
            {
                total = await Task.Run(() => { return se.QueryOver<Designation>().Future().Count(); });
                pager = new Pager(total, pagenum, pagesize);
                m = pager.GetItemMessage();
            }

            else
            {
                ICriteria cr = se.CreateCriteria<Designation>("des");
                GetFilterCriteria(cr, find, keyword);
                total = await Task.Run(() => { return cr.SetProjection(Projections.Count(Projections.Id())).UniqueResult<int>(); });
                pager = new Pager(total, pagenum, pagesize);
                m = pager.GetItemMessage();
            }

            return m;
        }

        private static void GetOrder(Sort sort, ICriteria cr)
        {
            bool sortDir = sort.Direction == "ASC" ? true : false;
            Order order = new Order(string.Format("des.{0}", sort.Column), sortDir);
            cr.AddOrder(order);
        }

        private static void GetFilterCriteria(ICriteria cr, int find, string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                if (find == 1)
                    cr.Add(Restrictions.InsensitiveLike("des.Title", keyword, MatchMode.Anywhere));

                else if (find == 2)
                    cr.Add(Restrictions.InsensitiveLike("des.Description", keyword, MatchMode.Anywhere));

                else if (find == 3)
                    cr.Add(Restrictions.InsensitiveLike("des.Note", keyword, MatchMode.Anywhere));

                else
                {
                    AbstractCriterion a1 = Restrictions.InsensitiveLike("des.Title", keyword, MatchMode.Anywhere);
                    AbstractCriterion a2 = Restrictions.InsensitiveLike("des.Description", keyword, MatchMode.Anywhere);
                    AbstractCriterion a3 = Restrictions.InsensitiveLike("des.Note", keyword, MatchMode.Anywhere);

                    AbstractCriterion b1 = Restrictions.Or(a1, a2);
                    AbstractCriterion b2 = Restrictions.Or(b1, a3);

                    cr.Add(b2);
                }
            }
        }
    }
}