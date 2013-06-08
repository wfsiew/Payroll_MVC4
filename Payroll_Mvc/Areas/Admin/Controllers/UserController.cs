using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Domain.Model;
using NHibernate;
using Payroll_Mvc.Models;
using Payroll_Mvc.Helpers;

namespace Payroll_Mvc.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /Admin/User/

        public ActionResult Index()
        {
            ListModel<User> l = null;

            l = UserHelper.GetAll();

            return View(l);
        }

        public ActionResult List()
        {
            string username = string.IsNullOrEmpty(Request["username"]) ? "" : Request["username"];
            int role = string.IsNullOrEmpty(Request["role"]) ? 0 : Convert.ToInt32(Request["role"]);
            string employee = string.IsNullOrEmpty(Request["employee"]) ? "" : Request["employee"];
            int status = string.IsNullOrEmpty(Request["status"]) ? 0 : Convert.ToInt32(Request["status"]);
            int pgnum = string.IsNullOrEmpty(Request["pgnum"]) ? 1 : Convert.ToInt32(Request["pgnum"]);
            int pgsize = string.IsNullOrEmpty(Request["pgsize"]) ? 0 : Convert.ToInt32(Request["pgsize"]);
            string sortcolumn = string.IsNullOrEmpty(Request["sortcolumn"]) ? UserHelper.DEFAULT_SORT_COLUMN : Request["sortcolumn"];
            string sortdir = string.IsNullOrEmpty(Request["sortdir"]) ? UserHelper.DEFAULT_SORT_DIR : Request["sortdir"];

            Sort sort = new Sort(sortcolumn, sortdir);

            Dictionary<string, object> filters = new Dictionary<string, object>
            {
                { "username", username },
                { "role", role },
                { "employee", employee },
                { "status", status }
            };

            ListModel<User> l = null;

            if (string.IsNullOrEmpty(username) && role == 0 && string.IsNullOrEmpty(employee) && status == 0)
                l = UserHelper.GetAll(pgnum, pgsize, sort);

            else
                l = UserHelper.GetFilterBy(filters, pgnum, pgsize, sort);

            return View("_list", l);
        }

        public ActionResult New()
        {
            ViewBag.form_id = "add-form";
            return View("_form", new User());
        }

        [HttpPost]
        public JsonResult Create(FormCollection fc)
        {
            User o = new User();
            o.SetProperties(fc);

            Dictionary<string, object> err = new Dictionary<string, object>();

            ISession se = NHibernateHelper.CurrentSession;
            err = o.IsValid(se);

            if (err.Keys.Count == 0)
            {
                using (ITransaction tx = se.BeginTransaction())
                {
                    o.EncryptPassword();
                    se.SaveOrUpdate(o);
                    tx.Commit();
                }

                return Json(new Dictionary<string, object>
                {
                    { "success", 1 },
                    { "message", "User was successfully added." }
                },
                JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(err, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Edit(Guid id)
        {
            User o = new User();
            ViewBag.form_id = "edit-form";

            ISession se = NHibernateHelper.CurrentSession;
            o = se.Get<User>(id);

            return View("_form", o);
        }

        [HttpPost]
        public JsonResult Update(Guid id, FormCollection fc)
        {
            Dictionary<string, object> err = new Dictionary<string, object>();
            User o = null;
            User x = new User();

            ISession se = NHibernateHelper.CurrentSession;

            o = se.Get<User>(id);
            x.SetProperties(fc);
            x.Id = id;

            err = x.IsValid(se);

            if (err.Keys.Count == 0)
            {
                using (ITransaction tx = se.BeginTransaction())
                {
                    o.Role = x.Role;
                    o.Username = x.Username;
                    o.Status = x.Status;

                    if (x.Password != Domain.Model.User.UNCHANGED_PASSWORD)
                    {
                        o.Password = x.Password;
                        o.EncryptPassword();
                    }

                    se.SaveOrUpdate(o);
                    tx.Commit();
                }

                return Json(new Dictionary<string, object>
                {
                    { "success", 1 },
                    { "message", "User was successfully updated." }
                },
                JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(err, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Delete(FormCollection fc)
        {
            string username = string.IsNullOrEmpty(Request["username"]) ? "" : Request["username"];
            int role = string.IsNullOrEmpty(Request["role"]) ? 0 : Convert.ToInt32(Request["role"]);
            string employee = string.IsNullOrEmpty(Request["employee"]) ? "" : Request["employee"];
            int status = string.IsNullOrEmpty(Request["status"]) ? 0 : Convert.ToInt32(Request["status"]);
            int pgnum = string.IsNullOrEmpty(Request["pgnum"]) ? 1 : Convert.ToInt32(Request["pgnum"]);
            int pgsize = string.IsNullOrEmpty(Request["pgsize"]) ? 0 : Convert.ToInt32(Request["pgsize"]);
            string ids = fc.Get("id[]");
            string[] idlist = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            string itemscount = null;

            Dictionary<string, object> filters = new Dictionary<string, object>
            {
                { "username", username },
                { "role", role },
                { "employee", employee },
                { "status", status }
            };

            ISession se = NHibernateHelper.CurrentSession;

            using (ITransaction tx = se.BeginTransaction())
            {
                se.CreateQuery("delete from User where id in (:idlist)")
                    .SetParameterList("idlist", idlist)
                    .ExecuteUpdate();
                tx.Commit();
            }

            if (string.IsNullOrEmpty(username) && role == 0 && string.IsNullOrEmpty(employee) && status == 0)
                itemscount = UserHelper.GetItemMessage(null, pgnum, pgsize);

            else
                itemscount = UserHelper.GetItemMessage(filters, pgnum, pgsize);
            
            return Json(new Dictionary<string, object>
            {
                { "success", 1 },
                { "itemscount", itemscount },
                { "message", string.Format("{0} user(s) was successfully deleted.", idlist.Length) }
            },
            JsonRequestBehavior.AllowGet);
        }
    }
}
