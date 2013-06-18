using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Tool.hbm2ddl;

using Domain.Model;

namespace Payroll_Seed
{
    class Program
    {
        static void Main(string[] args)
        {
            ISessionFactory s = CreateSessionFactory();

            CreateSeed(s);

            Console.WriteLine("Done");
            Console.ReadKey();
        }

        private static ISessionFactory CreateSessionFactory()
        {
            string online = "server=d22d5c2f-e928-4e03-aac0-a1da00acf6dd.mysql.sequelizer.com;database=dbd22d5c2fe9284e03aac0a1da00acf6dd;uid=jmemnjdpuwuksena;pwd=cteRnmTVvMGzf3pwYRD3vmErzQcmJoEjy7rk2FBiTvh2YQE4LzHaNLMYVUqEjFkg";
            string local = "Server=localhost;Port=3307;Database=testdb;Uid=root;Pwd=root";

            return Fluently.Configure()
                .Database(
                MySQLConfiguration.Standard
                .ConnectionString(online)
                .ShowSql()
                .AdoNetBatchSize(100)

                )
                .CurrentSessionContext("thread")
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.Load("Payroll.Domain")))
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
        }

        private static void BuildSchema(Configuration config)
        {
            new SchemaExport(config).Execute(true, true, false);
        }

        private static async Task CreateSeed(ISessionFactory s)
        {
            using (ISession se = s.OpenSession())
            {
                using (ITransaction tx = se.BeginTransaction())
                {
                    User a = new User { Username = "admin", Password = "admin123", Role = 1, Status = true };
                    a.EncryptPassword();
                    se.SaveOrUpdate(a);

                    a = new User { Username = "ben", Password = "ben123", Role = 2, Status = true };
                    a.EncryptPassword();
                    se.SaveOrUpdate(a);

                    Employee o = new Employee
                    {
                        Staffid = "S0001",
                        Firstname = "Ben",
                        Lastname = "Ng",
                        Newic = "77665544",
                        Gender = "M",
                        Maritalstatus = "S",
                        Nationality = "Malaysian",
                        Dob = new DateTime(1988, 6, 5),
                        Placeofbirth = "PJ",
                        Race = "Chinese",
                        Isbumi = false,
                        User = a
                    };
                    se.SaveOrUpdate(o);

                    Employeecontact ect = new Employeecontact
                    {
                        Id = o.Id,
                        Address1 = "No. 6, Jalan Awan Kecil 1",
                        Address2 = "Taman OUG",
                        Address3 = "Off Jalan Klang Lama",
                        City = "KL",
                        State = "WP",
                        Postcode = "58200",
                        Country = "Malaysia",
                        Homephone = "88098776",
                        Mobilephone = "77609887",
                        Workemail = "ben@synergy.com",
                        Otheremail = "ben@gmail.com",
                    };
                    se.SaveOrUpdate(ect);

                    await CreateListAttendance(se, o.Staffid, o);

                    Employeesalary es = new Employeesalary
                    {
                        Id = o.Id,
                        Salary = 0,
                        Allowance = 45,
                        Epf = 278,
                        Socso = 46,
                        Incometax = 57,
                        Bankname = "RHB",
                        Bankaccno = "5509800076",
                        Bankacctype = "Savings",
                        Bankaddress = "Jalan Awan Besar",
                        Epfno = "443987542",
                        Socsono = "8876908539",
                        Incometaxno = "439877055",
                        Paytype = 2
                    };
                    se.SaveOrUpdate(es);

                    Designation des = new Designation { Title = "Software Developer" };
                    se.SaveOrUpdate(des);

                    Department dept = new Department { Name = "R&D" };
                    se.SaveOrUpdate(dept);

                    Jobcategory jobcat = new Jobcategory { Name = "Software Development" };
                    se.SaveOrUpdate(jobcat);

                    Employmentstatus empstat = new Employmentstatus { Name = "Probation" };
                    se.SaveOrUpdate(empstat);

                    empstat = new Employmentstatus { Name = "Confirmed" };
                    se.SaveOrUpdate(empstat);

                    Employeejob empjob = new Employeejob
                    {
                        Id = o.Id,
                        Designation = des,
                        Department = dept,
                        Employmentstatus = empstat,
                        Jobcategory = jobcat,
                        Joindate = new DateTime(2000, 1, 1),
                        Confirmdate = new DateTime(2000, 3, 1)
                    };
                    se.SaveOrUpdate(empjob);

                    //

                    a = new User { Username = "ken", Password = "ken123", Role = 2, Status = true };
                    a.EncryptPassword();
                    se.SaveOrUpdate(a);

                    o = new Employee
                    {
                        Staffid = "S0002",
                        Firstname = "Ken",
                        Lastname = "Lee",
                        Newic = "785400",
                        Gender = "M",
                        Maritalstatus = "S",
                        Nationality = "Malaysian",
                        Dob = new DateTime(1986, 6, 5),
                        Placeofbirth = "PJ",
                        Race = "Chinese",
                        Isbumi = false,
                        User = a
                    };
                    se.SaveOrUpdate(o);

                    ect = new Employeecontact
                    {
                        Id = o.Id,
                        Address1 = "No. 7, Jalan Putra 1",
                        Address2 = "Taman Pinang",
                        Address3 = "Off Jalan Putra Perdana",
                        City = "KL",
                        State = "WP",
                        Postcode = "59200",
                        Country = "Malaysia",
                        Homephone = "88028776",
                        Mobilephone = "77659887",
                        Workemail = "ken@synergy.com",
                        Otheremail = "ken@gmail.com"
                    };
                    se.SaveOrUpdate(ect);

                    await CreateListAttendance(se, o.Staffid, o);

                    es = new Employeesalary
                    {
                        Id = o.Id,
                        Salary = 0,
                        Allowance = 55,
                        Epf = 298,
                        Socso = 65,
                        Incometax = 95,
                        Bankname = "RHB",
                        Bankaccno = "5509800077",
                        Bankacctype = "Savings",
                        Bankaddress = "Jalan Awan Besar",
                        Epfno = "443987548",
                        Socsono = "8878908539",
                        Incometaxno = "439899055",
                        Paytype = 2
                    };
                    se.SaveOrUpdate(es);

                    des = new Designation { Title = "Account Executive" };
                    se.SaveOrUpdate(des);

                    dept = new Department { Name = "Admin" };
                    se.SaveOrUpdate(dept);

                    jobcat = new Jobcategory { Name = "Administration" };
                    se.SaveOrUpdate(jobcat);

                    empjob = new Employeejob
                    {
                        Id = o.Id,
                        Designation = des,
                        Department = dept,
                        Employmentstatus = empstat,
                        Jobcategory = jobcat,
                        Joindate = new DateTime(2000, 2, 1),
                        Confirmdate = new DateTime(2000, 4, 1)
                    };
                    se.SaveOrUpdate(empjob);

                    //

                    a = new User { Username = "steve", Password = "steve123", Role = 2, Status = true };
                    a.EncryptPassword();
                    se.SaveOrUpdate(a);

                    o = new Employee
                    {
                        Staffid = "S0003",
                        Firstname = "Steve",
                        Lastname = "Yap",
                        Newic = "65098765",
                        Gender = "M",
                        Maritalstatus = "S",
                        Nationality = "Malaysian",
                        Dob = new DateTime(1974, 6, 5),
                        Placeofbirth = "PJ",
                        Race = "Chinese",
                        Isbumi = false,
                        User = a
                    };
                    se.SaveOrUpdate(o);

                    ect = new Employeecontact
                    {
                        Id = o.Id,
                        Address1 = "No. 5, Jalan Bukit Bintang",
                        Address2 = "Taman Bintang",
                        Address3 = "Off Jalan Bukit",
                        City = "KL",
                        State = "WP",
                        Postcode = "57200",
                        Country = "Malaysia",
                        Homephone = "88798776",
                        Mobilephone = "79609887",
                        Workemail = "steve@synergy.com",
                        Otheremail = "steve@gmail.com"
                    };
                    se.SaveOrUpdate(ect);

                    await CreateListAttendance(se, o.Staffid, o);

                    es = new Employeesalary
                    {
                        Id = o.Id,
                        Salary = 0,
                        Allowance = 55,
                        Epf = 300,
                        Socso = 62,
                        Incometax = 48,
                        Bankname = "RHB",
                        Bankaccno = "5509100076",
                        Bankacctype = "Savings",
                        Bankaddress = "Jalan Awan Besar",
                        Epfno = "473987542",
                        Socsono = "8879908539",
                        Incometaxno = "439817055",
                        Paytype = 2
                    };
                    se.SaveOrUpdate(es);

                    des = new Designation { Title = "Sales Executive" };
                    se.SaveOrUpdate(des);

                    dept = new Department { Name = "Sales" };
                    se.SaveOrUpdate(dept);

                    jobcat = new Jobcategory { Name = "Sales" };
                    se.SaveOrUpdate(jobcat);

                    empjob = new Employeejob
                    {
                        Id = o.Id,
                        Designation = des,
                        Department = dept,
                        Employmentstatus = empstat,
                        Jobcategory = jobcat,
                        Joindate = new DateTime(2000, 3, 1),
                        Confirmdate = new DateTime(2000, 5, 1)
                    };
                    se.SaveOrUpdate(empjob);

                    await CreatePayRate(se, "S0001");
                    await CreatePayRate(se, "S0002");
                    await CreatePayRate(se, "S0003");

                    //

                    a = new User { Username = "kelly", Password = "kelly123", Role = 2, Status = true };
                    a.EncryptPassword();
                    se.SaveOrUpdate(a);

                    o = new Employee
                    {
                        Staffid = "S0004",
                        Firstname = "Kelly",
                        Lastname = "Yap",
                        Newic = "55441122",
                        Gender = "F",
                        Maritalstatus = "S",
                        Nationality = "Malaysian",
                        Dob = new DateTime(1979, 6, 5),
                        Placeofbirth = "KL",
                        Race = "Chinese",
                        Isbumi = false,
                        User = a
                    };
                    se.SaveOrUpdate(o);

                    ect = new Employeecontact
                    {
                        Id = o.Id,
                        Address1 = "No. 2, Jalan Kerinchi 5",
                        Address2 = "Taman Bukit Kerinchi",
                        Address3 = "Off Jalan Kerinchi Besar",
                        City = "KL",
                        State = "WP",
                        Postcode = "56200",
                        Country = "Malaysia",
                        Homephone = "88098476",
                        Mobilephone = "77609187",
                        Workemail = "kelly@synergy.com",
                        Otheremail = "kelly@gmail.com"
                    };
                    se.SaveOrUpdate(ect);

                    await CreateListAttendance(se, o.Staffid, o);

                    Random r = new Random();

                    es = new Employeesalary
                    {
                        Id = o.Id,
                        Salary = r.Next(2500, 3000),
                        Allowance = r.Next(60, 100),
                        Epf = r.Next(100, 200),
                        Socso = r.Next(90, 100),
                        Incometax = r.Next(100, 200),
                        Bankname = "RHB",
                        Bankaccno = "667743290",
                        Bankacctype = "Savings",
                        Bankaddress = "Jalan Pinang",
                        Epfno = "59876000",
                        Socsono = "76545",
                        Incometaxno = "ASD965777",
                        Paytype = 1
                    };
                    se.SaveOrUpdate(es);

                    des = new Designation { Title = "Marketing Executive" };
                    se.SaveOrUpdate(des);

                    dept = new Department { Name = "Marketing" };
                    se.SaveOrUpdate(dept);

                    jobcat = new Jobcategory { Name = "Marketing" };
                    se.SaveOrUpdate(jobcat);

                    empjob = new Employeejob
                    {
                        Id = o.Id,
                        Designation = des,
                        Department = dept,
                        Employmentstatus = empstat,
                        Jobcategory = jobcat,
                        Joindate = new DateTime(2000, 1, 1),
                        Confirmdate = new DateTime(2000, 3, 1)
                    };
                    se.SaveOrUpdate(empjob);

                    await CreateOvertimeRate(se);
                    await CreateSalaryAdj(se, o.Staffid);

                    tx.Commit();
                }
            }
        }

        private static async Task CreateSalaryAdj(ISession se, string staffId)
        {
            Random r = new Random();

            await Task.Run(() =>
                {
                    for (int y = 2001; y <= DateTime.Now.Year; y++)
                    {
                        int x = r.Next(50, 90);
                        Salaryadjustment o = new Salaryadjustment { Staffid = staffId, Inc = x, Month = 1, Year = y };
                        se.SaveOrUpdate(o);
                    }
                });
        }

        private static async Task CreateOvertimeRate(ISession se)
        {
            Random r = new Random();

            await Task.Run(() =>
                {
                    for (int y = 2000; y <= DateTime.Now.Year; y++)
                    {
                        Overtimerate o = new Overtimerate { Duration = 1, Year = y, Payrate = r.Next(10, 50) };
                        se.SaveOrUpdate(o);
                    }
                });
        }

        private static async Task CreatePayRate(ISession se, string staffId)
        {
            Random r = new Random();

            await Task.Run(() =>
                {
                    for (int y = 2000; y <= DateTime.Now.Year; y++)
                    {
                        for (int m = 1; m <= 12; m++)
                        {
                            int x = r.Next(12, 18);
                            Payrate p = new Payrate { Staffid = staffId, Month = m, Year = y, Hourlypayrate = x };
                            se.SaveOrUpdate(p);
                        }
                    }
                });
        }

        private static async Task CreateListAttendance(ISession se, string staffId, Employee e)
        {
            Random r = new Random();

            await Task.Run(() =>
                {
                    for (int y = 2000; y <= DateTime.Now.Year; y++)
                    {
                        for (int m = 1; m <= 12; m++)
                        {
                            List<int> days = ListDay(m, y);
                            foreach (int k in days)
                            {
                                DateTime ti = new DateTime(y, m, k, 8, r.Next(20, 59), 0, DateTimeKind.Utc);
                                DateTime to = new DateTime(y, m, k, r.Next(18, 20), r.Next(0, 50), 0, DateTimeKind.Utc);
                                Attendance a = new Attendance { Staffid = staffId, Timein = ti, Timeout = to, Workdate = new DateTime(y, m, k) };
                                se.SaveOrUpdate(a);
                            }
                        }
                    }
                });
        }

        private static List<int> ListDay(int m, int y)
        {
            List<int> a = new List<int>();

            DateTime startdt = new DateTime(y, m, 1);
            DateTime enddt = startdt.AddMonths(1).AddDays(-1);

            for (int k = startdt.Day; k <= enddt.Day; k++)
            {
                DateTime v = new DateTime(y, m, k);
                if (v.DayOfWeek != DayOfWeek.Saturday && v.DayOfWeek != DayOfWeek.Sunday)
                    a.Add(k);
            }

            return a;
        }
    }
}
