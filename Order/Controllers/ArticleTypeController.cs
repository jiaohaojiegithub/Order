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
    [Category("ArticleType表")]
    [Description("ArticleType表-文章属性表")]
    public class ArticleTypeController : Controller
    {
        DBMYBLOGContext db = new DBMYBLOGContext();
        // GET: ArticleType
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet, ActionName("ArticleTypeInfo")]
        public JsonResult ArticleTypeInfo(int limit, int offset, string Search)
        {
            var total = 0;

            if (Search.Trim() == "")
            {
                var ArticleType = from a in db.ArticleType
                                  join b in db.Forum
                                  on a.Forum_ID equals b.Forum_ID
                                  into JionedEmpDept
                                  from b in JionedEmpDept.DefaultIfEmpty()
                                  select new
                                  {
                                      a.Forum_ID,
                                      a.ArticleType_ID,
                                      a.ArticleType_Name,
                                      b.Forum_Name
                                  };
                total = ArticleType.ToList().Count;
                var rows = ArticleType.Skip(offset).Take(limit).ToList();
                return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var ArticleType = from a in db.ArticleType
                                  join b in db.Forum
                                  on a.Forum_ID equals b.Forum_ID
                                  into JionedEmpDept
                                  from b in JionedEmpDept.DefaultIfEmpty()
                                  where a.ArticleType_Name.Contains(Search.Trim())
                                  select new
                                  {
                                      a.Forum_ID,
                                      a.ArticleType_ID,
                                      a.ArticleType_Name,
                                      b.Forum_Name
                                  };
                total = ArticleType.ToList().Count;
                var rows = ArticleType.Skip(offset).Take(limit).ToList();
                return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost, ActionName("DeleteConfirmed")]
        //[ValidateAntiForgeryToken]
        public int DeleteConfirmed(int id)
        {
            var state = 0;
            try
            {
                ArticleType articleType = db.ArticleType.Find(id);
                db.ArticleType.Remove(articleType);
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
        [HttpPost, ActionName("CreatArticleType")]
        public int CreatArticleType(string ArticleType_Name, string Forum_Text)
        {
            var state = 0;
            try {
                if (ArticleType_Name.Trim() != "" && Forum_Text.Trim() != "")
                {
                    if (db.ArticleType.Where(x => x.ArticleType_Name == ArticleType_Name.Trim()).Count() > 0)
                    {
                        state = 2;
                    }
                    else
                    {
                        ArticleType articleType;
                        int Forum_ID;
                        if (db.Forum.Where(x => x.Forum_Name == Forum_Text.Trim()).Count() > 0)
                        {
                            Forum_ID = db.Forum.Where(x => x.Forum_Name == Forum_Text.Trim()).Select(x => x.Forum_ID).FirstOrDefault();
                            articleType = new ArticleType { Forum_ID = Forum_ID, ArticleType_Name = ArticleType_Name.Trim()+"-"+ Forum_Text.Trim() };
                            db.ArticleType.Add(articleType);
                            db.SaveChanges();
                        }
                        else
                        {
                            Forum forum = new Forum { Forum_Name = Forum_Text.Trim() };
                            db.SaveChanges();
                            Forum_ID = db.Forum.Where(x => x.Forum_Name == Forum_Text.Trim()).Select(x => x.Forum_ID).FirstOrDefault();
                            articleType = new ArticleType { Forum_ID = Forum_ID, ArticleType_Name = ArticleType_Name.Trim()+"-"+ Forum_Text.Trim() };
                            db.ArticleType.Add(articleType);
                            db.SaveChanges();
                        }
                        state = 0;
                    }
                }
                else
                {
                    state = 1;
                }
            }
            catch (DataException)
            {
                state = 5;
                ModelState.AddModelError("", "无法保存更改。再试一次，如果问题仍然存在，请参见系统管理员。");
            }
            return state;
        }
        [HttpPost, ActionName("EditArticleType")]
        public int EditArticleType(string ArticleType_Name, string Forum_Text,int id)
        {
            var state = 0;
            ArticleType articleType;
            int Forum_ID;
            try {
                if (ArticleType_Name.Trim() != "" && Forum_Text.Trim() != "")
                {
                    if (db.ArticleType.Where(x => x.ArticleType_Name == ArticleType_Name.Trim()).Count() > 0)
                    {
                        //Forum_ID = db.Forum.Where(x => x.Forum_Name == Forum_Text.Trim()).Select(x => x.Forum_ID).FirstOrDefault();
                        //articleType = new ArticleType { Forum_ID = Forum_ID,ArticleType_ID = id };
                        //db.Entry(articleType).State = EntityState.Modified;
                        //db.SaveChanges();
                        state = 2;
                    }
                    else
                    {
                        
                        
                        Forum_ID = db.Forum.Where(x => x.Forum_Name == Forum_Text.Trim()).Select(x => x.Forum_ID).FirstOrDefault();
                        articleType = new ArticleType { Forum_ID = Forum_ID, ArticleType_Name = ArticleType_Name.Trim()+"-"+ Forum_Text.Trim(), ArticleType_ID=id };
                        db.Entry(articleType).State = EntityState.Modified;
                        db.SaveChanges();
                        state = 0;
                    }
                }
                else
                {
                    state = 1;
                }
            }
            catch (DataException)
            {
                state = 5;
                ModelState.AddModelError("", "无法保存更改。再试一次，如果问题仍然存在，请参见系统管理员。");
            }
            return state;
        }
        [HttpPost, ActionName("SelectArticleType")]
        public JsonResult SelectArticleType()
        {
            var list = db.ArticleType.ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost, ActionName("SelectArticleTypeList")]
        [Category("导航栏")]
        [Description("子菜单导航栏")]
        public JsonResult SelectArticleTypeList(int id)
        {
            var d = from a in db.ArticleType
                    where a.Forum_ID == id
                    select new
                    {
                        id = a.ArticleType_ID,
                        name = a.ArticleType_Name
                    };
            return Json(d.ToList(), JsonRequestBehavior.AllowGet);
        }
        #region 数据测试
        public JsonResult tset()
        {
            var ArticleType = from a in db.ArticleType
                              join b in db.Forum
                              on a.Forum_ID equals b.Forum_ID
                              into JionedEmpDept
                              from b in JionedEmpDept.DefaultIfEmpty()
                              select new
                              {
                                  a.Forum_ID,
                                  a.ArticleType_ID,
                                  a.ArticleType_Name,
                                  b.Forum_Name
                              };
            return Json(new { total = ArticleType.ToList().Count, rows = ArticleType.ToList() }, JsonRequestBehavior.AllowGet);
        }
        #endregion
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