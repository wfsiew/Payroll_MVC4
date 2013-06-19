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
using Payroll_Mvc.Attributes;

namespace Payroll_Mvc.Areas.Admin.Controllers
{
    [Authorize]
    [AuthAdmin]
    public class SalaryAdjustmentController : AsyncController
    {
        //
        // GET: /Admin/SalaryAdjustment/

        public async Task<ActionResult> Index()
        {
            ListModel<Salaryadjustment> l = null;

            l = await SalaryadjustmentHelper.GetAll();

            return View(l);
        }

        public async Task<ActionResult> List()
        {
            string staff_id = CommonHelper.GetValue(Request["staff_id"]);
            int month = CommonHelper.GetValue<int>(Request["month"], 0);
            int year = CommonHelper.GetValue<int>(Request["year"], 0);
            int pgnum = CommonHelper.GetValue<int>(Request["pgnum"], 1);
            int pgsize = CommonHelper.GetValue<int>(Request["pgsize"], 0);
            string sortcolumn = CommonHelper.GetValue(Request["sortcolumn"], SalaryadjustmentHelper.DEFAULT_SORT_COLUMN);
            string sortdir = CommonHelper.GetValue(Request["sortdir"], SalaryadjustmentHelper.DEFAULT_SORT_DIR);

            Sort sort = new Sort(sortcolumn, sortdir);

            Dictionary<string, object> filters = new Dictionary<string, object>
            {
                { "staff_id", staff_id },
                { "month", month },
                { "year", year }
            };

            ListModel<Salaryadjustment> l = null;

            if (string.IsNullOrEmpty(staff_id) && month == 0 && year == 0)
                l = await SalaryadjustmentHelper.GetAll(pgnum, pgsize, sort);

            else
                l = await SalaryadjustmentHelper.GetFilterBy(filters, pgnum, pgsize, sort);

            return View("_list", l);
        }

        public ActionResult New()
        {
            ViewBag.form_id = "add-form";
            return View("_form", new Salaryadjustment());
        }

        [HttpPost]
        public async Task<JsonResult> Create(FormCollection fc)
        {
            Salaryadjustment o = SalaryadjustmentHelper.GetObject(null, fc);

            Dictionary<string, object> err = null;

            ISession se = NHibernateHelper.CurrentSession;
            err = o.IsValid();

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
                    { "message", "Salary Adjustment was successfully added." }
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
            Salaryadjustment o = await Task.Run(() => { return se.Get<Salaryadjustment>(id); });

            return View("_form", o);
        }

        [HttpPost]
        public async Task<JsonResult> Update(Guid id, FormCollection fc)
        {
            Dictionary<string, object> err = null;
            Salaryadjustment o = null;

            ISession se = NHibernateHelper.CurrentSession;

            o = await Task.Run(() => { return se.Get<Salaryadjustment>(id); });
            o = SalaryadjustmentHelper.GetObject(o, fc);

            err = o.IsValid();

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
                    { "message", "Salary Adjustment was successfully updated." }
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
            string staff_id = CommonHelper.GetValue(Request["staff_id"]);
            int month = CommonHelper.GetValue<int>(Request["month"], 0);
            int year = CommonHelper.GetValue<int>(Request["year"], 0);
            int pgnum = CommonHelper.GetValue<int>(Request["pgnum"], 1);
            int pgsize = CommonHelper.GetValue<int>(Request["pgsize"], 0);
            string ids = fc.Get("id[]");
            string[] idlist = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            string itemscount = null;

            Dictionary<string, object> filters = new Dictionary<string, object>
            {
                { "staff_id", staff_id },
                { "month", month },
                { "year", year }
            };

            ISession se = NHibernateHelper.CurrentSession;

            await Task.Run(() =>
                {
                    using (ITransaction tx = se.BeginTransaction())
                    {
                        se.CreateQuery("delete from Salaryadjustment where id in (:idlist)")
                            .SetParameterList("idlist", idlist)
                            .ExecuteUpdate();
                        tx.Commit();
                    }
                });

            if (string.IsNullOrEmpty(staff_id) && month == 0 && year == 0)
                itemscount = await SalaryadjustmentHelper.GetItemMessage(null, pgnum, pgsize);

            else
                itemscount = await SalaryadjustmentHelper.GetItemMessage(filters, pgnum, pgsize);

            return Json(new Dictionary<string, object>
            {
                { "success", 1 },
                { "itemscount", itemscount },
                { "message", string.Format("{0} salary adjustment(s) was successfully deleted.", idlist.Length) }
            },
            JsonRequestBehavior.AllowGet);
        }
    }
}
