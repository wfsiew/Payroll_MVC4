using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    }
}