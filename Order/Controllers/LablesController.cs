using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
#region ADDUsing
using Order.Models;
using Order.Models.DBMYBLOGClass;
using System.ComponentModel;
using System.Data;
using System.Net;
using Microsoft.EntityFrameworkCore;
#endregion
namespace Order.Controllers
{
    [Category("Lables表")]
    [Description("Lables表-标签表")]
    public class LablesController : Controller
    {
        DBMYBLOGContext db = new DBMYBLOGContext();
        // GET: Lables
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 标签详细信息-暂时不需要此项功能
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Lables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lable lable = db.Lable.Find(id);
            if (lable == null)
            {
                return HttpNotFound();
            }
            return View(lable);
        }
        /// <summary>
        /// 新建标签视图
        /// </summary>
        /// <returns></returns>
        // GET: Lables/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetLablesInfo(int limit, int offset)//,string LableText
        {
            List<Lable> lables = db.Lable.ToList();
            //if (LableText.Trim() != "")
            //{
            //    //List<Lable> lables = db.Lable.Skip(offset).Take(limit).ToList();
            //    lables = lables.FindAll(x => x.Lable_Text.Contains(LableText));
            //}
            var total = lables.Count;
            var rows = lables.Skip(offset).Take(limit).ToList();
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Lable_ID,Lable_CreatDT,Lable_Remark,Lable_Text")] Lable lable)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Lable.Add(lable);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "无法保存更改。再试一次，如果问题仍然存在，请参见系统管理员。");
            }

            return View(lable);
            //return RedirectToAction("Index", lable);
        }
        [HttpPost,ActionName("Edit")]
        //[ValidateAntiForgeryToken]
        public int Edit(string Lable_Remark,string Lable_Text,string inputLable_ID)
        {
            var state = 0;
            try
            {
                int id = int.Parse(inputLable_ID.Trim());
                if(Lable_Remark.Trim()==""|| Lable_Text.Trim()=="")
                {
                    state = 1;
                }else
                if (db.Lable.Where(x => x.Lable_Text == Lable_Text.Trim()).Count() > 0)
                {
                    state = 2;
                }
                else
                {
                    Lable lable = new Lable { Lable_ID = id, Lable_Remark = Lable_Remark.Trim(), Lable_Text = Lable_Text.Trim(),Lable_CreatDT=DateTime.Now };
                    db.Entry(lable).State = EntityState.Modified;
                    db.SaveChanges();
                    state = 0;
                }
                //if (ModelState.IsValid)
                //{
                //    db.Entry(lable).State = EntityState.Modified;
                //    db.SaveChanges();
                //    return RedirectToAction("Index");
                //}
            }
            catch (DataException)
            {
                state = 5;
                ModelState.AddModelError("", "无法保存更改。再试一次，如果问题仍然存在，请参见系统管理员。");
            }
            return state;
        }
        // POST: Lables/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        public int DeleteConfirmed(int id)
        {
            var state = 0;
            try
            {
                Lable lable = db.Lable.Find(id);
                db.Lable.Remove(lable);
                db.SaveChanges();
                state = 0;
            }
            catch (DataException)
            {
                state = 5;
                ModelState.AddModelError("", "无法保存更改。再试一次，如果问题仍然存在，请参见系统管理员。");
            }
            return state;
        }
        /// <summary>
        /// 查询标签
        /// </summary>
        /// <returns></returns>
        [HttpPost, ActionName("SelectALLLable")]
        public JsonResult SelectALLLable()
        {
            var d = from a in db.Lable
                        //where a.Forum_ID == id
                    select new
                    {
                        id = a.Lable_ID,
                        name = a.Lable_Text
                    };
            return Json(d.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}