using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

using Domain.Model;
using NHibernate;
using Payroll_Mvc.Models;
using Payroll_Mvc.Helpers;

namespace Payroll_Mvc.Areas.Admin.Controllers
{
    [Authorize]
    public class UserController : AsyncController
    {
        //
        // GET: /Admin/User/

        public async Task<ActionResult> Index()
        {
            ListModel<User> l = null;

            l = await UserHelper.GetAll();

            return View(l);
        }

        public async Task<ActionResult> List()
        {
            string username = CommonHelper.GetValue(Request["username"]);
            int role = CommonHelper.GetValue<int>(Request["role"], 0);
            string employee = CommonHelper.GetValue(Request["employee"]);
            int status = CommonHelper.GetValue<int>(Request["status"], 0);
            int pgnum = CommonHelper.GetValue<int>(Request["pgnum"], 1);
            int pgsize = CommonHelper.GetValue<int>(Request["pgsize"], 0);
            string sortcolumn = CommonHelper.GetValue(Request["sortcolumn"], UserHelper.DEFAULT_SORT_COLUMN);
            string sortdir = CommonHelper.GetValue(Request["sortdir"], UserHelper.DEFAULT_SORT_DIR);

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
                l = await UserHelper.GetAll(pgnum, pgsize, sort);

            else
                l = await UserHelper.GetFilterBy(filters, pgnum, pgsize, sort);

            return View("_list", l);
        }

        public ActionResult New()
        {
            ViewBag.form_id = "add-form";
            return View("_form", new User());
        }

        [HttpPost]
        public async Task<JsonResult> Create(FormCollection fc)
        {
            User o = UserHelper.GetObject(fc);

            Dictionary<string, object> err = null;

            ISession se = NHibernateHelper.CurrentSession;
            err = o.IsValid(se);

            if (err == null)
            {
                await Task.Run(() =>
                    {
                        using (ITransaction tx = se.BeginTransaction())
                        {
                            o.EncryptPassword();
                            se.SaveOrUpdate(o);
                            tx.Commit();
                        }
                    });

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

        public async Task<ActionResult> Edit(Guid id)
        {
            ViewBag.form_id = "edit-form";

            ISession se = NHibernateHelper.CurrentSession;
            User o = await Task.Run(() => { return se.Get<User>(id); });

            return View("_form", o);
        }

        [HttpPost]
        public async Task<JsonResult> Update(Guid id, FormCollection fc)
        {
            Dictionary<string, object> err = null;
            User o = null;
            User x = UserHelper.GetObject(fc);

            ISession se = NHibernateHelper.CurrentSession;

            o = await Task.Run(() => { return se.Get<User>(id); });
            x.Id = id;

            err = x.IsValid(se);

            if (err == null)
            {
                await Task.Run(() =>
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
                    });

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
        public async Task<JsonResult> Delete(FormCollection fc)
        {
            string username = CommonHelper.GetValue(Request["username"]);
            int role = CommonHelper.GetValue<int>(Request["role"], 0);
            string employee = CommonHelper.GetValue(Request["employee"]);
            int status = CommonHelper.GetValue<int>(Request["status"], 0);
            int pgnum = CommonHelper.GetValue<int>(Request["pgnum"], 1);
            int pgsize = CommonHelper.GetValue<int>(Request["pgsize"], 0);
            string ids = fc.Get("id[]");
            string[] idlist = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            string itemscount = null;

            ISession se = NHibernateHelper.CurrentSession;

            await DeleteReferences(se, idlist);

            Dictionary<string, object> filters = new Dictionary<string, object>
            {
                { "username", username },
                { "role", role },
                { "employee", employee },
                { "status", status }
            };

            await Task.Run(() =>
                {
                    using (ITransaction tx = se.BeginTransaction())
                    {
                        se.CreateQuery("delete from User where id in (:idlist)")
                            .SetParameterList("idlist", idlist)
                            .ExecuteUpdate();
                        tx.Commit();
                    }
                });

            if (string.IsNullOrEmpty(username) && role == 0 && string.IsNullOrEmpty(employee) && status == 0)
                itemscount = await UserHelper.GetItemMessage(null, pgnum, pgsize);

            else
                itemscount = await UserHelper.GetItemMessage(filters, pgnum, pgsize);
            
            return Json(new Dictionary<string, object>
            {
                { "success", 1 },
                { "itemscount", itemscount },
                { "message", string.Format("{0} user(s) was successfully deleted.", idlist.Length) }
            },
            JsonRequestBehavior.AllowGet);
        }

        private async Task DeleteReferences(ISession se, string[] idlist)
        {
            foreach (string id in idlist)
            {
                Guid uid = new Guid(id);
                Domain.Model.User o = se.Get<User>(uid);
                Employee e = o.Employee;

                if (e != null)
                {
                    e.User = null;

                    await Task.Run(() =>
                        {
                            using (ITransaction tx = se.BeginTransaction())
                            {
                                se.SaveOrUpdate(e);
                                tx.Commit();
                            }
                        });
                }
            }
        }
    }
}
