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
    [Category("Forum表")]
    [Description("Forum表-数据分类表")]
    public class ForumController : Controller
    {
        DBMYBLOGContext db = new DBMYBLOGContext();
        // GET: Forum
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet, ActionName("ForumInfo")]
        public JsonResult ForumInfo(int limit, int offset,string Search)
        {
            var total = 0;
            var rows = new List<Forum>();
            if (Search.Trim() == "")
            {
                List<Forum> forums = db.Forum.ToList();
                total = forums.Count;
                rows = forums.Skip(offset).Take(limit).ToList();
            }
            else
            {
                List<Forum> forums = db.Forum.Where(x=>x.Forum_Name.Contains(Search.Trim())).ToList();
                total = forums.Count;
                rows = forums.Skip(offset).Take(limit).ToList();
            }
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost, ActionName("CreatForum")]
        [Authorize(Roles = "Admin")]
        public int CreatForum(string Forum_Text)
        {
            var state = 0;
            try
            {
                if (Forum_Text.Trim() != "")
                {
                    if (db.Forum.Where(x => x.Forum_Name == Forum_Text.Trim()).Count() > 0)
                    {
                        state = 2;//类别已经存在
                    }
                    else
                    {
                        Forum forum = new Forum { Forum_Name = Forum_Text.Trim() };
                        db.Forum.Add(forum);
                        db.SaveChanges();
                        state = 0;

                    }
                }
                else
                {
                    state = 1;//为空
                }
            }
            catch (DataException)
            {
                state = 5;
                ModelState.AddModelError("", "无法保存更改。再试一次，如果问题仍然存在，请参见系统管理员。");
            }
            return state;

        }
        [HttpPost, ActionName("Edit")]
        [Authorize(Roles = "Admin")]
        public int Edit(string Forum_ID, string Forum_Text)
        {
            var state = 0;
            try
            {
                int id = int.Parse(Forum_ID.Trim());
                if (Forum_ID.Trim() != "" && Forum_Text.Trim() != "")
                {
                    if (db.Lable.Where(x => x.Lable_Text == Forum_Text.Trim()).Count() > 0)
                    {
                        state = 2;
                    }
                    else
                    {
                        Forum lable = new Forum { Forum_Name=Forum_Text.Trim()};
                        db.Entry(lable).State = EntityState.Modified;
                        db.SaveChanges();
                        state = 0;
                    }
                }
                else
                {
                    state = 1;//数据为空
                }

            }
            catch (DataException)
            {
                state = 5;
                ModelState.AddModelError("", "无法保存更改。再试一次，如果问题仍然存在，请参见系统管理员。");
            }
            return state;
        }
        [HttpPost, ActionName("DeleteConfirmed")]
        public int DeleteConfirmed(int id)
        {
            var state = 0;
            try
            {
                Forum forum = db.Forum.Find(id);
                db.Forum.Remove(forum);
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
        [HttpPost, ActionName("SelectForum")]
        [Category("导航栏")]
        [Description("主菜单导航栏")]
        public JsonResult SelectForum()
        {
            var list = db.Forum.ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
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