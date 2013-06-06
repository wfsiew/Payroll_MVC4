using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System.Reflection;

namespace Payroll_Mvc.Models
{
    public class NHibernateHelper
    {
        private static ISessionFactory SessionFactory { get; set; }
        private static bool StartupComplete { get; set; }
        private static readonly object locker = new object();

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        public static void EnsureStartup()
        {
            if (!StartupComplete)
            {
                lock (locker)
                {
                    if (!StartupComplete)
                    {
                        PerformStartup();
                        StartupComplete = true;
                    }
                }
            }
        }

        private static void PerformStartup()
        {
            InitSessionFactory();
        }

        private static void InitSessionFactory()
        {
            FluentConfiguration cfg = BuildConfiguration();
            SessionFactory = cfg.BuildSessionFactory();
        }

        private static FluentConfiguration BuildConfiguration()
        {
            FluentConfiguration cfg = Fluently.Configure()
                .Database(MySQLConfiguration.Standard.ConnectionString("Server=localhost;Port=3307;Database=test;Uid=root;Pwd=root")
                .ShowSql()
                .AdoNetBatchSize(100))
                .CurrentSessionContext("web")
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.Load("Payroll.Domain")))
                .ExposeConfiguration(BuildSchema);
            return cfg;  
        }

        private static void BuildSchema(Configuration config)
        {
            new SchemaUpdate(config)
            .Execute(true, true);
        }
    }
}