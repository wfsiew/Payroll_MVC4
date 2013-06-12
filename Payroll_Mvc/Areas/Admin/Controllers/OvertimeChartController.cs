﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NHibernate;
using NHibernate.Criterion;

using Domain.Model;
using Payroll_Mvc.Models;
using Payroll_Mvc.Helpers;
using Payroll_Mvc.Areas.Admin.Models;

namespace Payroll_Mvc.Areas.Admin.Controllers
{
    public class OvertimeChartController : Controller
    {
        //
        // GET: /Admin/OvertimeChart/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Data()
        {
            string staff_id = string.IsNullOrEmpty(Request["staff_id"]) ? "" : Request["staff_id"];
            string _month = Request["month"];
            string _year = string.IsNullOrEmpty(Request["year"]) ? "0" : Request["year"];

            if (string.IsNullOrEmpty(_month))
                _month = Request["month[]"];

            if (string.IsNullOrEmpty(_month))
                _month = "0";

            string title = "Overtime";
            string yaxis = "Duration (hours)";

            ISession se = NHibernateHelper.CurrentSession;

            ICriteria cr = se.CreateCriteria<Attendance>();

            if (!string.IsNullOrEmpty(staff_id))
                cr.Add(Restrictions.Eq("Staffid", staff_id));

            if (_year != "0")
            {
                int year = Convert.ToInt32(_year);
                IProjection yearProjection = Projections.SqlFunction("year", NHibernateUtil.Int32, Projections.Property("Workdate"));
                cr.Add(Restrictions.Eq(yearProjection, year));
                title = string.Format("Overtime for Year {0}", year);
            }

            if (_month != "0")
            {
                string[] monthlist = _month.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                IProjection monthProjection = Projections.SqlFunction("month", NHibernateUtil.Int32, Projections.Property("Workdate"));
                cr.Add(Restrictions.In(monthProjection, monthlist));
            }

            List<Attendance> list = cr.List<Attendance>().ToList();

            string[] categories = new string[12];
            for (int i = 1; i < 13; i++)
            {
                categories[i - 1] = CommonHelper.GetAbbreviatedMonthName(i);
            }

            double[] b = new double[12];

            foreach (Attendance o in list)
            {
                DateTime to = o.Timeout.GetValueOrDefault();
                DateTime v = new DateTime(to.Year, to.Month, to.Day, 18, 0, 0, DateTimeKind.Utc);
                double x = (to - v).TotalSeconds / 3600.0;
                int m = o.Workdate.GetValueOrDefault().Month;

                if (x > 0)
                    b[m - 1] += x;
            }

            double[] c = new double[12];
            for (int i = 0; i < b.Length; i++)
            {
                c[i] = Math.Round(b[i], 2);
            }

            return Json(new Dictionary<string, object>
            {
                { "data", c },
                { "categories", categories },
                { "title", title },
                { "yaxis", yaxis }
            },
            JsonRequestBehavior.AllowGet);
        }
    }
}
