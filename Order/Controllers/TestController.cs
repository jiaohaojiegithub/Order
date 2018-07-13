using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
#region ADDUsing
using Order.Models;
using Order.Models.DBMYBLOGClass;
using Order.Controllers;
using System.Data;
#endregion
namespace Order.Controllers
{
    public class TestController : Controller
    {
        DBMYBLOGContext db = new DBMYBLOGContext();
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TestUeditor()
        {
            return View();
        }
        public ActionResult TestBootstrapTreegrid()
        {
            return View();
        }
        public ActionResult TestBlogs()
        {
            return View();
        }
        public ActionResult Ordinary()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult OrdinaryRegister()
        {
            return View();
        }
        public ActionResult TestTexiao()
        {
            return View();
        }
        public ActionResult TestArticleInfo_Ordinary()
        {
            return View();
        }
        public ActionResult TestChangyan()
        {
            ViewBag.ID = 1;
            return View();
        }
        public ActionResult TestComment()
        {
            User_Login user_Login = new User_Login();
            var user_Logins = from a in db.User_Login
                              where a.UserLogin_ID == 1
                              select a;
            foreach (var a in user_Logins)
            {
                user_Login = a;
            }
            Session["User"] = user_Login;//user_Login;
            User_Login login = (User_Login)Session["User"];
            ViewBag.ID = login.UserLogin_ID;
            ViewBag.Article_ID = 1;//文章id
            return View();//评论模块
        }
        /// <summary>
        /// 获取文章评论
        /// </summary>
        /// <param name="id">文章编号</param>
        /// <returns></returns>
        [HttpPost, ActionName("CommentALL")]
        public JsonResult CommentALL(int id)
        {
            var CommentData = (from c in db.Comment
                               join uc in db.User_Card on c.UserCard_ID equals uc.UserCard_ID
                               join ul in db.User_Login on uc.UserLogin_ID equals ul.UserLogin_ID
                               where c.Article_ID == id
                               select new
                               {
                                   c.Comment_ID,
                                   c.Article_ID,
                                   c.Comment_Contene,
                                   创建时间 = c.Comment_CreatDT.ToString("yyyy-MM-dd hh:mm:ss"),
                                   c.UserCard_ID,
                                   uc.UserCard_GmLevel,
                                   uc.UserCard_ChatHeadImg,
                                   ul.UserLogin_Name,
                                   ul.UserLogin_ID
                               });
            return Json(CommentData.ToList(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost, ActionName("Comment_ReplyALL")]
        public JsonResult Comment_ReplyALL(int id)
        {
            var CommentData = (from c in db.Comment_Reply
                               join uc in db.User_Card on c.UserCard_ID equals uc.UserCard_ID
                               join ul in db.User_Login on uc.UserLogin_ID equals ul.UserLogin_ID
                               where c.Comment_ID == id
                               select new
                               {
                                   c.Comment_ID,
                                   c.Comment_Reply_ID,
                                   c.Comment_Reply_Contene,
                                   创建时间 = c.Comment_Reply_CreatDT.ToString("yyyy-MM-dd hh:mm:ss"),
                                   c.UserCard_ID,
                                   uc.UserCard_GmLevel,
                                   uc.UserCard_ChatHeadImg,
                                   ul.UserLogin_Name,
                                   ul.UserLogin_ID
                               });
            return Json(CommentData.ToList(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost, ActionName("CommentADD")]
        public int CommentADD(string Contene, int ArticleID, int UserCardID)
        {
            var state = 0;
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    Comment comment = new Comment
                    {
                        Article_ID = ArticleID,
                        Comment_Contene = Contene,
                        UserCard_ID = UserCardID
                    };
                    db.Comment.Add(comment);
                    db.SaveChanges();
                    dbContextTransaction.Commit();
                    state = 10;//添加成功
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    state = 5;
                    ModelState.AddModelError("", "无法保存更改。再试一次，如果问题仍然存在，请参见系统管理员。");
                }
            }
            return state;
        }
        [HttpPost, ActionName("Comment_ReplyADD")]
        public int Comment_ReplyADD(int CommentID, string CommentReplyContene, int UserCardID)
        {
            var state = 0;
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    Comment_Reply comment_Reply = new Comment_Reply
                    {
                        Comment_ID = CommentID,
                        Comment_Reply_Contene = CommentReplyContene,
                        UserCard_ID = UserCardID
                    };
                    db.Comment_Reply.Add(comment_Reply);
                    db.SaveChanges();
                    dbContextTransaction.Commit();
                    state = 10;//添加成功
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    state = 5;
                    ModelState.AddModelError("", "无法保存更改。再试一次，如果问题仍然存在，请参见系统管理员。");
                }
            }
            return state;
        }
    }
}