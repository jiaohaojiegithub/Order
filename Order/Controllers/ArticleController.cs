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
using Newtonsoft.Json.Linq;
using System.Text;
#endregion
namespace Order.Controllers
{
    [Category("Article表")]
    [Description("Article表-文章表")]
    public class ArticleController : Controller
    {
        DBMYBLOGContext db = new DBMYBLOGContext();
        // GET: Article
        public ActionResult Index(string id)
        {

            //JObject jo1 = (JObject)JsonConvert.DeserializeObject(urlname.ToString());
            //foreach (var i in article)
            //{
            //    i.创建时间;
            //}
            //ViewBag.List = article;//Json(article.ToList(),JsonRequestBehavior.AllowGet);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                if (id == "0")
                {
                    ViewBag.state = "";
                }
                else
                {
                    ViewBag.state = id;
                }
            }
            //ViewBag.state = "";
            return View();
        }
        public ActionResult IndexS(string ArticleType_Name)
        {
            if (ArticleType_Name == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                if (ArticleType_Name == "0")
                {
                    ViewBag.state = "";
                }
                else
                {
                    ViewBag.state = ArticleType_Name;
                }
            }
            return PartialView();
        }
        /// <summary>
        /// 文章详细表
        /// </summary>
        /// <param name="id">文章ID</param>
        /// <returns></returns>
        public ActionResult ArticleInfo_ID(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                if (id == 0)
                {
                    var article = from a in db.Article
                                  join b in db.ArticleType on a.ArticleType_ID equals b.ArticleType_ID
                                  into JionedEmpDept
                                  from ab in JionedEmpDept.DefaultIfEmpty()
                                  join c in db.User_Card on a.UserCard_ID equals c.UserCard_ID
                                  into JionedEmpDeptac
                                  from ac in JionedEmpDeptac.DefaultIfEmpty()
                                      //where ab.ArticleType_Name.Contains(ArticleType_Name)
                                  select new
                                  {
                                      ID = a.Article_ID,
                                      文章标题 = a.Article_Title,
                                      文章描述 = a.Article_Abstract,
                                      文章内容 = a.Article_Content,
                                      文章封面 = a.Article_Img,
                                      创建时间 = a.Article_CreateDT,
                                      文章标签 = a.Lable_ID,
                                      文章类型 = ab.ArticleType_Name,
                                      文章作者 = ac.UserCard_Nickname
                                  };
                    foreach (var i in article)
                    {
                        ViewBag.Article_ID = i.ID;
                        ViewData["文章标题"] = i.文章标题;
                        ViewData["文章描述"] = i.文章描述;
                        ViewData["文章内容"] = i.文章内容.ToString();
                        ViewData["创建时间"] = i.创建时间;
                        string[] lables = i.文章标签.ToString().Split(',');
                        string[] lablestr=new string[lables.Length];
                        for (int a = 0; a < lables.Length; a++)
                        {
                            int lableID = int.Parse(lables[a]);
                            lablestr[a] = db.Lable.Where(x => x.Lable_ID == lableID).Select(x => x.Lable_Text).FirstOrDefault();
                        }
                        ViewData["文章标签"] = lablestr;
                        ViewData["文章类型"] = i.文章类型;
                        ViewData["文章作者"] = i.文章作者;
                    }
                }
                else
                {
                    var article = from a in db.Article
                                  join b in db.ArticleType on a.ArticleType_ID equals b.ArticleType_ID
                                  into JionedEmpDept
                                  from ab in JionedEmpDept.DefaultIfEmpty()
                                  join c in db.User_Card on a.UserCard_ID equals c.UserCard_ID
                                  into JionedEmpDeptac
                                  from ac in JionedEmpDeptac.DefaultIfEmpty()
                                  where a.Article_ID==id
                                  select new
                                  {
                                      ID = a.Article_ID,
                                      文章标题 = a.Article_Title,
                                      文章描述 = a.Article_Abstract,
                                      文章内容 = a.Article_Content,
                                      文章封面 = a.Article_Img,
                                      创建时间 = a.Article_CreateDT,
                                      文章标签 = a.Lable_ID,
                                      文章类型 = ab.ArticleType_Name,
                                      文章作者 = ac.UserCard_Nickname
                                  };
                    foreach (var i in article)
                    {
                        ViewBag.Article_ID = i.ID;
                        ViewData["文章标题"] = i.文章标题;
                        ViewData["文章描述"] = i.文章描述;
                        ViewData["文章内容"] = i.文章内容.ToString();
                        ViewData["创建时间"] = i.创建时间;
                        string[] lables = i.文章标签.ToString().Split(',');
                        string[] lablestr = new string[lables.Length];
                        for (int a = 0; a < lables.Length; a++)
                        {
                            //int lableID = int.Parse(lables[a+1]);
                            lablestr[a] = db.Lable.Where(x => x.Lable_ID.ToString()==lables[a]).Select(x => x.Lable_Text).FirstOrDefault();
                        }
                        ViewData["文章标签"] = lablestr;
                        ViewData["文章类型"] = i.文章类型;
                        ViewData["文章作者"] = i.文章作者;
                        ViewBag.Labe = lablestr;
                        

                    }
                }
            }
            return View();
        }
        [HttpPost, ActionName("ArticleInfo")]
        public JsonResult ArticleInfo(string ArticleType_Name, int limit, int offset)
        {
            var article = from a in db.Article
                          join b in db.ArticleType on a.ArticleType_ID equals b.ArticleType_ID
                          into JionedEmpDept
                          from ab in JionedEmpDept.DefaultIfEmpty()
                          join c in db.User_Card on a.UserCard_ID equals c.UserCard_ID
                          into JionedEmpDeptac
                          from ac in JionedEmpDeptac.DefaultIfEmpty()
                          orderby a.Article_CreateDT descending
                          //where ab.ArticleType_Name.Contains(ArticleType_Name)
                          select new
                          {
                              ID = a.Article_ID,
                              文章标题 = a.Article_Title,
                              文章描述 = a.Article_Abstract,
                              文章内容 = a.Article_Content,
                              文章封面 = a.Article_Img,
                              创建时间 = a.Article_CreateDT.ToString("yyyy/MM/dd"),
                              文章标签 = a.Lable_ID,
                              文章类型 = ab.ArticleType_Name,
                              文章作者 = ac.UserCard_Nickname
                          };
            var total = article.Count();
            var rows = article.Where(x => x.文章类型.Contains(ArticleType_Name)).Skip(offset).Take(limit).ToList();
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 查询所有文章
        /// </summary>
        /// <param name="limit">每页列数</param>
        /// <param name="offset">从第几行开始，跳过指定行数</param>
        /// <param name="SearchArticleName">文章名查询</param>
        /// <param name="SearchArticleTypeName">文章类型查询</param>
        /// <returns></returns>
        [HttpGet, ActionName("ArticleInfo_ALL")]
        public JsonResult ArticleInfo_ALL(int limit, int offset, string SearchArticleName, string SearchArticleTypeName,int id)
        {
            var ArticleInfoALL= from a in db.Article
                                join b in db.ArticleType on a.ArticleType_ID equals b.ArticleType_ID
                                into JionedEmpDept
                                from ab in JionedEmpDept.DefaultIfEmpty()
                                join c in db.User_Card on a.UserCard_ID equals c.UserCard_ID
                                into JionedEmpDeptac
                                from ac in JionedEmpDeptac.DefaultIfEmpty()
                                where a.Article_Title.Contains(SearchArticleName) && ab.ArticleType_Name.Contains(SearchArticleTypeName)
                                select new
                                {
                                    ID = a.Article_ID,
                                    文章标题 = a.Article_Title,
                                    文章描述 = a.Article_Abstract,
                                    文章内容 = a.Article_Content,
                                    文章封面 = a.Article_Img,
                                    创建时间 = a.Article_CreateDT.ToString("yyyy/MM/dd"),
                                    文章标签 = a.Lable_ID,
                                    文章类型 = ab.ArticleType_Name,
                                    文章作者 = ac.UserCard_Nickname,
                                    UCID=ac.UserLogin_ID
                                };
            if (id == 0)
            {

            }
            else {
                ArticleInfoALL = from a in ArticleInfoALL
                                 where a.UCID == id
                                 select a;
            }
            var total = ArticleInfoALL.ToList().Count;
            var rows = ArticleInfoALL.Skip(offset).Take(limit).ToList();
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="id">文章ID</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,PUTONG")]
        [HttpPost, ActionName("DeleteConfirmed")]
        //[ValidateAntiForgeryToken]
        public int DeleteConfirmed(int id)
        {
            var state = 0;
            try
            {
                Article article = db.Article.Find(id);
                db.Article.Remove(article);
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
        //导出word
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ExportWord(string html)
        {
            StringBuilder word = new StringBuilder();
            word.Append("<!DOCTYPE html>");
            word.Append("<body>");
            word.Append(html);
            word.Append("</body>");
            var byteArray = System.Text.Encoding.Default.GetBytes(word.ToString());
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            return File(byteArray, "application/ms-word", "文章" + Guid.NewGuid() + ".doc");
            //DirectoryInfo directory = new System.IO.DirectoryInfo(Directory.GetCurrentDirectory());
            //string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);// directory.Parent.Parent.FullName + @"\ExeclData";
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);

            //}
            //string pathfile = path + @"\" + DateTime.Now.ToString("yyyy-MM-dd") + ".doc";
            //return File(byteArray, "application/ms-word", pathfile);
        }
        //[HttpPost,ActionName("ArticleInfo_ID")]
        [Description("释放资源")]
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