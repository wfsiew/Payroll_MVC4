using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Threading.Tasks;

using NHibernate;
using Domain.Model;
using Payroll_Mvc.Helpers;

namespace Payroll_Mvc.Controllers
{
    public class ApplicationController : AsyncController
    {
        //
        // GET: /Application/

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(FormCollection fc)
        {
            string username = fc.Get("username");
            string password = fc.Get("password");

            ISession se = NHibernateHelper.CurrentSession;
            User user = await Task.Run(() => { return Domain.Model.User.Authenticate(se, username, password); });

            if (user != null)
            {
                Session["user_id"] = user.Id;
                if (user.Role == Domain.Model.User.ADMIN)
                {
                    FormsAuthentication.SetAuthCookie(username, true);
                    return RedirectToRoute("Admin_index");
                }

                else
                {
                    Employee employee = user.Employee;
                    if (employee != null)
                    {
                        Session["employee_id"] = employee.Id;
                        Session["staff_id"] = employee.Staffid;
                        FormsAuthentication.SetAuthCookie(username, true);
                        return RedirectToRoute("User_index");
                    }

                    else
                        ViewBag.alert = @"No employee record found. Please contact the administrator 
                                          to create your employee record.";
                }
            }

            else
                ViewBag.alert = "Incorrect username or password";

            return View("Login");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}
