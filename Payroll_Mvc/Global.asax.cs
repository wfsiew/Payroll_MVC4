using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

using NHibernate;
using NHibernate.Context;
using Payroll_Mvc.Helpers;

namespace Payroll_Mvc
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            NHibernateHelper.EnsureStartup();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            ISession s = NHibernateHelper.OpenSession();
            CurrentSessionContext.Bind(s);
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            ISession s = CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            s.Dispose();
        }
    }
}