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
    public class DepartmentController : AsyncController
    {
        //
        // GET: /Admin/Department/

        public async Task<ActionResult> Index()
        {
            ListModel<Department> l = null;

            l = await DepartmentHelper.GetAll();

            return View(l);
        }

        public async Task<ActionResult> List()
        {
            string keyword = CommonHelper.GetValue(Request["keyword"]);
            int pgnum = CommonHelper.GetValue<int>(Request["pgnum"], 1);
            int pgsize = CommonHelper.GetValue<int>(Request["pgsize"], 0);
            string sortcolumn = CommonHelper.GetValue(Request["sortcolumn"], DepartmentHelper.DEFAULT_SORT_COLUMN);
            string sortdir = CommonHelper.GetValue(Request["sortdir"], DepartmentHelper.DEFAULT_SORT_DIR);

            Sort sort = new Sort(sortcolumn, sortdir);

            ListModel<Department> l = null;

            if (string.IsNullOrEmpty(keyword))
                l = await DepartmentHelper.GetAll(pgnum, pgsize, sort);

            else
                l = await DepartmentHelper.GetFilterBy(keyword, pgnum, pgsize, sort);

            return View("_list", l);
        }

        public ActionResult New()
        {
            ViewBag.form_id = "add-form";
            return View("_form", new Department());
        }

        [HttpPost]
        public async Task<ActionResult> Create(FormCollection fc)
        {
            Department o = DepartmentHelper.GetObject(null, fc);

            Dictionary<string, object> err = null;

            ISession se = NHibernateHelper.CurrentSession;
            err = o.IsValid(se);

            if (err == null)
            {
                await Task.Run(() =>
                {
                    using (ITransaction tx = se.BeginTransaction())
                    {
                        se.SaveOrUpdate(o);
                        tx.Commit();
                    }
                });

                return Json(new Dictionary<string, object>
                {
                    { "success", 1 },
                    { "message", "Department was successfully added." }
                },
                JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(err, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            ViewBag.form_id = "edit-form";

            ISession se = NHibernateHelper.CurrentSession;
            Department o = await Task.Run(() => { return se.Get<Department>(id); });

            return View("_form", o);
        }

        [HttpPost]
        public async Task<JsonResult> Update(int id, FormCollection fc)
        {
            Dictionary<string, object> err = null;
            Department o = null;

            ISession se = NHibernateHelper.CurrentSession;

            o = await Task.Run(() => { return se.Get<Department>(id); });
            o = DepartmentHelper.GetObject(o, fc);

            err = o.IsValid(se);

            if (err == null)
            {
                await Task.Run(() =>
                {
                    using (ITransaction tx = se.BeginTransaction())
                    {
                        se.SaveOrUpdate(o);
                        tx.Commit();
                    }
                });

                return Json(new Dictionary<string, object>
                {
                    { "success", 1 },
                    { "message", "Department was successfully updated." }
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
            string keyword = CommonHelper.GetValue(Request["keyword"]);
            int pgnum = CommonHelper.GetValue<int>(Request["pgnum"], 1);
            int pgsize = CommonHelper.GetValue<int>(Request["pgsize"], 0);
            string ids = fc.Get("id[]");
            string[] idlist = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            string itemscount = null;

            ISession se = NHibernateHelper.CurrentSession;

            await DeleteReferences(se, idlist);

            await Task.Run(() =>
            {
                using (ITransaction tx = se.BeginTransaction())
                {
                    se.CreateQuery("delete from Department where id in (:idlist)")
                        .SetParameterList("idlist", idlist)
                        .ExecuteUpdate();
                    tx.Commit();
                }
            });

            itemscount = await DepartmentHelper.GetItemMessage(keyword, pgnum, pgsize);

            return Json(new Dictionary<string, object>
            {
                { "success", 1 },
                { "itemscount", itemscount },
                { "message", string.Format("{0} Department(s) was successfully deleted.", idlist.Length) }
            },
            JsonRequestBehavior.AllowGet);
        }

        private async Task DeleteReferences(ISession se, string[] idlist)
        {
            foreach (string id in idlist)
            {
                int uid = CommonHelper.GetValue<int>(id);
                Department o = se.Get<Department>(uid);
                IList<Employeejob> l = o.Employeejob;

                if (l != null)
                {
                    foreach (Employeejob e in l)
                    {
                        e.Department = null;

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
}
