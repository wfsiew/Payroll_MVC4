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
    public class EmployeeHelper
    {
        public const string DEFAULT_SORT_COLUMN = "Staffid";
        public const string DEFAULT_SORT_DIR = "ASC";

        public static ListModel<Employee> GetAll(int pagenum = 1, int pagesize = Pager.DEFAULT_PAGE_SIZE,
            Sort sort = null)
        {
            ListModel<Employee> l = new ListModel<Employee>();

            if (sort == null)
                sort = new Sort(DEFAULT_SORT_COLUMN, DEFAULT_SORT_DIR);

            ISession se = NHibernateHelper.CurrentSession;
            int total = se.QueryOver<Employee>().Future().Count();
            Pager pager = new Pager(total, pagenum, pagesize);

            int has_next = pager.HasNext ? 1 : 0;
            int has_prev = pager.HasPrev ? 1 : 0;

            ICriteria cr = se.CreateCriteria<Employee>("employee");

            if (sort.Column == "d.Title" || sort.Column == "es.Name" ||
                sort.Column == "dept.Name")
                SetJoinCriteria(cr, sort);

            GetOrder(sort, cr);

            cr.SetFirstResult(pager.LowerBound);
            cr.SetMaxResults(pager.PageSize);

            List<Employee> list = cr.List<Employee>().ToList();

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

        public static ListModel<Employee> GetFilterBy(Dictionary<string, object> filters, int pagenum = 1,
            int pagesize = Pager.DEFAULT_PAGE_SIZE, Sort sort = null)
        {
            ListModel<Employee> l = new ListModel<Employee>();

            if (sort == null)
                sort = new Sort(DEFAULT_SORT_COLUMN, DEFAULT_SORT_DIR);

            ISession se = NHibernateHelper.CurrentSession;
            ICriteria cr = se.CreateCriteria<Employee>("employee");
            GetFilterCriteria(cr, filters, sort);

            ICriteria crCount = se.CreateCriteria<Employee>("employee");
            GetFilterCriteria(crCount, filters, sort);

            int total = crCount.SetProjection(Projections.Count(Projections.Id())).UniqueResult<int>();
            Pager pager = new Pager(total, pagenum, pagesize);

            int has_next = pager.HasNext ? 1 : 0;
            int has_prev = pager.HasPrev ? 1 : 0;

            GetOrder(sort, cr);

            cr.SetFirstResult(pager.LowerBound);
            cr.SetMaxResults(pager.PageSize);

            List<Employee> list = cr.List<Employee>().ToList();

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

        private static void GetOrder(Sort sort, ICriteria cr)
        {
            bool sortDir = sort.Direction == "ASC" ? true : false;
            Order order = null;

            if (sort.Column.IndexOf(".", StringComparison.OrdinalIgnoreCase) >= 0)
                order = new Order(sort.Column, sortDir);

            else
                order = new Order(string.Format("employee.{0}", sort.Column), sortDir);

            cr.AddOrder(order);
        }

        private static void GetFilterCriteria(ICriteria cr, Dictionary<string, object> filters, Sort sort = null)
        {
            string employee = Convert.ToString(filters["employee"]);
            string staff_id = Convert.ToString(filters["staff_id"]);
            int employment_status = (int)filters["employment_status"];
            int designation = (int)filters["designation"];
            int dept = (int)filters["dept"];

            if (!string.IsNullOrEmpty(employee))
            {
                AbstractCriterion a1 = Restrictions.InsensitiveLike("employee.Firstname", employee, MatchMode.Anywhere);
                AbstractCriterion a2 = Restrictions.InsensitiveLike("employee.Middlename", employee, MatchMode.Anywhere);
                AbstractCriterion a3 = Restrictions.InsensitiveLike("employee.Lastname", employee, MatchMode.Anywhere);

                AbstractCriterion b1 = Restrictions.Or(a1, a2);
                AbstractCriterion b2 = Restrictions.Or(b1, a3);

                cr.Add(b2);
            }

            if (!string.IsNullOrEmpty(staff_id))
                cr.Add(Restrictions.InsensitiveLike("employee.Staffid", staff_id, MatchMode.Anywhere));

            if (employment_status != 0)
            {
                cr = cr.CreateCriteria("employee.Employeejob", "ej", JoinType.InnerJoin);
                cr = cr.CreateCriteria("ej.Employmentstatus", "es", JoinType.InnerJoin);
                cr.Add(Restrictions.Eq("es.Id", employment_status));
            }

            if (designation != 0)
            {
                ICriteria crej = cr.GetCriteriaByAlias("ej");
                if (crej == null)
                    cr = cr.CreateCriteria("employee.Employeejob", "ej", JoinType.InnerJoin);

                else
                    cr = crej;

                cr = cr.CreateCriteria("ej.Designation", "d", JoinType.InnerJoin);

                cr.Add(Restrictions.Eq("d.Id", designation));
            }

            if (dept != 0)
            {
                ICriteria crej = cr.GetCriteriaByAlias("ej");
                if (crej == null)
                    cr = cr.CreateCriteria("employee.Employeejob", "ej", JoinType.InnerJoin);

                else
                    cr = crej;

                cr = cr.CreateCriteria("ej.Department", "dept", JoinType.InnerJoin);

                cr.Add(Restrictions.Eq("dept.Id", dept));
            }

            if (sort != null)
                SetJoinCriteria(cr, sort);
        }

        private static void SetJoinCriteria(ICriteria cr, Sort sort)
        {
            ICriteria crej = cr.GetCriteriaByAlias("ej");

            if (sort.Column == "d.Title")
            {
                ICriteria crd = cr.GetCriteriaByAlias("d");
                if (crej != null && crd == null)
                    cr = crej.CreateCriteria("ej.Designation", "d", JoinType.LeftOuterJoin);

                else if (crej == null && crd == null)
                {
                    crej = cr.CreateCriteria("employee.Employeejob", "ej", JoinType.LeftOuterJoin);
                    cr = crej.CreateCriteria("ej.Designation", "d", JoinType.LeftOuterJoin);
                }
            }

            if (sort.Column == "es.Name")
            {
                ICriteria cres = cr.GetCriteriaByAlias("es");
                if (crej != null && cres == null)
                    cr = crej.CreateCriteria("employee.Employeejob", "ej", JoinType.LeftOuterJoin);

                else if (crej == null && cres == null)
                {
                    crej = cr.CreateCriteria("employee.Employeejob", "ej", JoinType.LeftOuterJoin);
                    cr = crej.CreateCriteria("ej.Employmentstatus", "es", JoinType.LeftOuterJoin);
                }
            }

            if (sort.Column == "dept.Name")
            {
                ICriteria crdept = cr.GetCriteriaByAlias("dept");
                if (crej != null && crdept == null)
                    cr = crej.CreateCriteria("ej.Department", "dept", JoinType.LeftOuterJoin);

                else if (crej == null && crdept == null)
                {
                    crej = cr.CreateCriteria("employee.Employeejob", "ej", JoinType.LeftOuterJoin);
                    cr = crej.CreateCriteria("ej.Department", "dept", JoinType.LeftOuterJoin);
                }
            }
        }
    }
}