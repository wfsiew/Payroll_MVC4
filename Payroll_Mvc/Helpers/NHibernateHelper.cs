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

namespace Payroll_Mvc.Helpers
{
    public class NHibernateHelper
    {
        public static ISessionFactory SessionFactory { get; private set; }
        private static bool StartupComplete { get; set; }
        private static readonly object locker = new object();

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        public static ISession CurrentSession
        {
            get
            {
                return SessionFactory.GetCurrentSession();
            }
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
            string online = "server=d22d5c2f-e928-4e03-aac0-a1da00acf6dd.mysql.sequelizer.com;database=dbd22d5c2fe9284e03aac0a1da00acf6dd;uid=jmemnjdpuwuksena;pwd=cteRnmTVvMGzf3pwYRD3vmErzQcmJoEjy7rk2FBiTvh2YQE4LzHaNLMYVUqEjFkg";
            string local = "Server=localhost;Port=3307;Database=testdb;Uid=root;Pwd=root";

            FluentConfiguration cfg = Fluently.Configure()
                .Database(MySQLConfiguration.Standard.ConnectionString(local)
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