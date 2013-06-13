using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

using Domain.Model;
using Payroll_Mvc.Models;
using Payroll_Mvc.Helpers;
using Payroll_Mvc.Areas.Admin.Models;

namespace Payroll_Mvc.Areas.Admin.Controllers
{
    public class HourlyPayrollChartController : Controller
    {
        //
        // GET: /Admin/HourlyPayrollChart/

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

            string title = "Hourly Payroll";
            string yaxis = "Total Amount (RM)";

            object[] o = new object[12];
            double[] b = new double[12];
            string[] categories = new string[12];

            for (int i = 1; i < 13; i++)
            {
                o[i - 1] = new object[,] { { CommonHelper.GetMonthName(i), 0 } };
                categories[i - 1] = CommonHelper.GetAbbreviatedMonthName(i);
            }

            ISession se = NHibernateHelper.CurrentSession;

            ICriteria cr = se.CreateCriteria<Payrate>();

            List<string> liststaff = new List<string>();
            List<int> listyear = new List<int>();
            List<int> listmonth = new List<int>();

            if (!string.IsNullOrEmpty(staff_id))
                liststaff.Add(staff_id);

            else
            {
                cr.SetProjection(Projections.Distinct(Projections.ProjectionList()
                    .Add(Projections.Alias(Projections.Property("Staffid"), "Staffid"))));

                cr.SetResultTransformer(new AliasToBeanResultTransformer(typeof(Payrate)));

                IList<Payrate> list = cr.List<Payrate>();

                foreach (Payrate x in list)
                    liststaff.Add(x.Staffid);
            }

            if (_year != "0")
            {
                int year = Convert.ToInt32(_year);
                listyear.Add(year);
                title = string.Format("Hourly Payroll for {0}", year);
            }

            else
            {
                cr.SetProjection(Projections.Distinct(Projections.ProjectionList()
                    .Add(Projections.Alias(Projections.Property("Year"), "Year"))));

                cr.SetResultTransformer(new AliasToBeanResultTransformer(typeof(Payrate)));

                IList<Payrate> list = cr.List<Payrate>();

                foreach (Payrate x in list)
                    listyear.Add(x.Year);
            }

            if (_month != "0")
            {
                string[] monthlist = _month.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string x in monthlist)
                    listmonth.Add(Convert.ToInt32(x));
            }

            else
            {
                for (int i = 1; i < 13; i++)
                    listmonth.Add(i);
            }

            foreach (int y in listyear)
            {
                foreach (int m in listmonth)
                {
                    foreach (string s in liststaff)
                    {
                        Dictionary<string, object> filters = new Dictionary<string, object>
                        {
                            { "year", y },
                            { "month", m },
                            { "staff_id", s }
                        };

                        double total_hours = AttendanceHelper.GetTotalHours(filters);
                        double rate = PayrateHelper.GetPayRate(filters);
                        double v = total_hours * rate;
                        object[,] t = o[m - 1] as object[,];
                        t[0, 1] = v;
                        b[m - 1] += v;
                    }
                }
            }

            double[] c = new double[12];
            for (int i = 0; i < b.Length; i++)
            {
                c[i] = Math.Round(b[i], 2);
            }

            return Json(new Dictionary<string, object>
            {
                { "pie", o },
                { "column", new Dictionary<string, object>
                    {
                        { "data", c },
                        { "categories", categories },
                        { "title", title },
                        { "yaxis", yaxis }
                    }
                }
            },
            JsonRequestBehavior.AllowGet);
        }
    }
}
