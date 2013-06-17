using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

using Domain.Model;
using Payroll_Mvc.Models;
using Payroll_Mvc.Helpers;
using Payroll_Mvc.Areas.Admin.Models;

namespace Payroll_Mvc.Areas.Admin.Controllers
{
    [Authorize]
    public class TotalWorkHoursChartController : AsyncController
    {
        //
        // GET: /Admin/TotalWorkHoursChart/

        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> Data()
        {
            string staff_id = CommonHelper.GetValue(Request["staff_id"]);
            string _month = Request["month"];
            string _year = CommonHelper.GetValue(Request["year"], "0");

            if (string.IsNullOrEmpty(_month))
                _month = Request["month[]"];

            if (string.IsNullOrEmpty(_month))
                _month = "0";

            string title = "Total Hours Worked";
            string yaxis = "Duration (hours)";

            double[] b = new double[12];
            string[] categories = new string[12];
            double[] c = new double[12];

            for (int i = 1; i < 13; i++)
            {
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

                IList<Payrate> list = await Task.Run(() => { return cr.List<Payrate>(); });

                await Task.Run(() =>
                    {
                        foreach (Payrate x in list)
                            liststaff.Add(x.Staffid);
                    });
            }

            if (_year != "0")
            {
                int year = Convert.ToInt32(_year);
                listyear.Add(year);
                title = string.Format("Total Hours Worked for {0}", year);
            }

            else
            {
                cr.SetProjection(Projections.Distinct(Projections.ProjectionList()
                    .Add(Projections.Alias(Projections.Property("Year"), "Year"))));

                cr.SetResultTransformer(new AliasToBeanResultTransformer(typeof(Payrate)));

                IList<Payrate> list = await Task.Run(() => { return cr.List<Payrate>(); });

                await Task.Run(() =>
                    {
                        foreach (Payrate x in list)
                            listyear.Add(x.Year);
                    });
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

                        double total_hours = await AttendanceHelper.GetTotalHours(filters);
                        double v = total_hours;
                        b[m - 1] += v;
                    }
                }
            }

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
