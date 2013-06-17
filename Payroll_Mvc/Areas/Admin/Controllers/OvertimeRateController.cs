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
    public class OvertimeRateController : AsyncController
    {
        //
        // GET: /Admin/OvertimeRate/

        public async Task<ActionResult> Index()
        {
            ListModel<Overtimerate> l = null;

            l = await OvertimerateHelper.GetAll();

            return View(l);
        }

        public async Task<ActionResult> List()
        {
            int year = CommonHelper.GetValue<int>(Request["year"], 0);
            int pgnum = CommonHelper.GetValue<int>(Request["pgnum"], 1);
            int pgsize = CommonHelper.GetValue<int>(Request["pgsize"], 0);
            string sortcolumn = CommonHelper.GetValue(Request["sortcolumn"], OvertimerateHelper.DEFAULT_SORT_COLUMN);
            string sortdir = CommonHelper.GetValue(Request["sortdir"], OvertimerateHelper.DEFAULT_SORT_DIR);

            Sort sort = new Sort(sortcolumn, sortdir);

            Dictionary<string, object> filters = new Dictionary<string, object>
            {
                { "year", year }
            };

            ListModel<Overtimerate> l = null;

            if (year == 0)
                l = await OvertimerateHelper.GetAll(pgnum, pgsize, sort);

            else
                l = await OvertimerateHelper.GetFilterBy(filters, pgnum, pgsize, sort);

            return View("_list", l);
        }

        public ActionResult New()
        {
            ViewBag.form_id = "add-form";
            return View("_form", new Overtimerate());
        }

        [HttpPost]
        public async Task<JsonResult> Create(FormCollection fc)
        {
            Overtimerate o = OvertimerateHelper.GetObject(null, fc);

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
                    { "message", "Overtime Rate  was successfully added." }
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
            Overtimerate o = await Task.Run(() => { return se.Get<Overtimerate>(id); });

            return View("_form", o);
        }

        [HttpPost]
        public async Task<JsonResult> Update(int id, FormCollection fc)
        {
            Dictionary<string, object> err = null;
            Overtimerate o = null;

            ISession se = NHibernateHelper.CurrentSession;

            o = await Task.Run(() => { return se.Get<Overtimerate>(id); });
            o = OvertimerateHelper.GetObject(o, fc);

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
                    { "message", "Overtime Rate  was successfully updated." }
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
            int year = CommonHelper.GetValue<int>(Request["year"], 0);
            int pgnum = CommonHelper.GetValue<int>(Request["pgnum"], 1);
            int pgsize = CommonHelper.GetValue<int>(Request["pgsize"], 0);
            string ids = fc.Get("id[]");
            string[] idlist = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            string itemscount = null;

            Dictionary<string, object> filters = new Dictionary<string, object>
            {
                { "year", year }
            };

            ISession se = NHibernateHelper.CurrentSession;

            await Task.Run(() =>
            {
                using (ITransaction tx = se.BeginTransaction())
                {
                    se.CreateQuery("delete from Overtimerate where id in (:idlist)")
                        .SetParameterList("idlist", idlist)
                        .ExecuteUpdate();
                    tx.Commit();
                }
            });

            if (year == 0)
                itemscount = await OvertimerateHelper.GetItemMessage(null, pgnum, pgsize);

            else
                itemscount = await OvertimerateHelper.GetItemMessage(filters, pgnum, pgsize);

            return Json(new Dictionary<string, object>
            {
                { "success", 1 },
                { "itemscount", itemscount },
                { "message", string.Format("{0} Overtime rate(s) was successfully deleted.", idlist.Length) }
            },
            JsonRequestBehavior.AllowGet);
        }
    }
}
