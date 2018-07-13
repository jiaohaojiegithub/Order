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
    public class User_LoginController : Controller
    {
        DBMYBLOGContext db = new DBMYBLOGContext();
        [Authorize(Roles ="Admin")]
        [Category("UserLogin表")]
        [Description("UserLogin表-列出用户信息")]
        // GET: User_Login
        public ActionResult Index()
        {
            return PartialView(db.User_Login.ToList());
        }
        [Authorize(Roles = "Admin")]
        // GET: User_Login/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_Login user_Login = db.User_Login.Find(id);
            if (user_Login == null)
            {
                return HttpNotFound();
            }
            return View(user_Login);
            //return RedirectToAction("Index", "Admin");
        }
        [Authorize(Roles = "Admin")]
        // GET: User_Login/Create
        public ActionResult Create()
        {
            return View();
        }
        /// <summary>
        ///创建新用户
        /// </summary>
        /// <param name="user_Login"></param>
        /// <returns></returns>
        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "UserLogin_ID,UserLogin_CreatDT,UserLogin_Guid,UserLogin_Name,UserLogin_PassWord,UserLogin_State")] User_Login user_Login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.User_Login.Add(user_Login);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "无法保存更改。再试一次，如果问题仍然存在，请参见系统管理员。");
            }

            return View(user_Login);
        }

        // GET: User_Login/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_Login user_Login = db.User_Login.Find(id);
            if (user_Login == null)
            {
                return HttpNotFound();
            }
            return View(user_Login);
        }
        /// <summary>
        /// 修改用户数据
        /// </summary>
        /// <param name="user_Login"></param>
        /// <returns></returns>
        [HttpPost,ActionName("Edit")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserLogin_ID,UserLogin_CreatDT,UserLogin_Guid,UserLogin_Name,UserLogin_PassWord,UserLogin_State")] User_Login user_Login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(user_Login).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "无法保存更改。再试一次，如果问题仍然存在，请参见系统管理员。");
            }
            return View(user_Login);
        }
        // GET: User_Login/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_Login user_Login = db.User_Login.Find(id);
            if (user_Login == null)
            {
                return HttpNotFound();
            }
            return View(user_Login);
        }
        [Authorize(Roles = "Admin")]
        // POST: User_Login/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                User_Login user_Login = db.User_Login.Find(id);
                db.User_Login.Remove(user_Login);
                db.SaveChanges();
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "无法保存更改。再试一次，如果问题仍然存在，请参见系统管理员。");
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}