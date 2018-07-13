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
using System.IO;
#endregion
namespace Order.Controllers
{
    public class ManagementOrdinaryController : Controller
    {
        DBMYBLOGContext db = new DBMYBLOGContext();
        public bool start = false;
        /// <summary>
        /// 普通用户的用户中心
        /// </summary>
        /// <returns></returns>
        // GET: ManagementOrdinary
        [Authorize(Roles = "Admin,PUTONG")]
        public ActionResult Index(int? id)
        {
            if (id != null)
            {
                CheckLoginStart();
                if (start)
                {
                    ViewBag.ID = id;
                    return View();
                }
                else
                {

                    ViewBag.ID = 0;
                    return RedirectToRoute(new { Controller = "ManagementOrdinary", action = "Index" });
                    //User_Login user_Login = (User_Login)Session["Username"];
                    //return Content(user_Login.UserLogin_Name);
                }
            }
            else
            {
                return RedirectToRoute(new { Controller = "ManagementOrdinary", action = "Index" });
            }
        }
        [Description("新增文章-普通用户")]
        [Authorize(Roles = "Admin,PUTONG")]
        public ActionResult ArticleADD_Ordinary()
        {
            //添加测试用初始数据
            return View();
        }
        [Description("文章详细列表-普通用户")]
        [Authorize(Roles = "Admin,PUTONG")]
        public ActionResult ArticleInfo_Ordinary(int? id)
        {
            if (id == null)
            {
                ViewBag.ID = 0;
            }
            else
            {
                ViewBag.ID = id;
            }
            return View();
        }
        [Authorize(Roles = "Admin,PUTONG")]
        public ActionResult Article_Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                if (id == 0)
                {
                    ViewBag.state = "";
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
                                  where a.Article_ID == id
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
                        ViewData["文章标题"] = i.文章标题;
                        ViewData["文章描述"] = i.文章描述;
                        ViewData["文章内容"] = i.文章内容.ToString();
                        ViewData["创建时间"] = i.创建时间;
                        var lables = i.文章标签.Split(',');
                        string LablesID_str = "";
                        foreach (var lable in lables)
                        {
                            LablesID_str = LablesID_str + db.Lable.Where(x => x.Lable_ID.ToString() == lable).Select(x => x.Lable_Text).FirstOrDefault() + ",";
                        }
                        ViewData["文章标签"] = LablesID_str;
                        ViewData["文章类型"] = i.文章类型;
                        ViewData["文章作者"] = i.文章作者;
                        ViewData["文章封面"] = i.文章封面;
                        ViewBag.Labe = LablesID_str;


                    }
                    ViewBag.state = id;
                }
            }
            //ViewBag.state = "";
            return View();
            //return Content(id.ToString());
        }
        [HttpPost, ActionName("ArticleADDTest")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Authorize(Roles = "Admin,PUTONG")]
        public ActionResult ArticleADDTest(FormCollection collection)
        {
            var array = collection["biaoqian"];//.Split(',');
            return Content(array);
        }
        [HttpPost, ActionName("ArticleADD")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Authorize(Roles = "Admin,PUTONG")]
        public ActionResult ArticleADD(FormCollection collection)
        {
            try
            {
                User_Login user_Login1 = new User_Login();
                var user_Logins = from a in db.User_Login
                                  where a.UserLogin_ID == 1
                                  select a;
                foreach (var a in user_Logins)
                {
                    user_Login1 = a;
                }
                Session["UsernameOrdinary"] = user_Login1;
                var content = collection["editor"];
                string PathImgFile = "";
                User_Login user_Login = new User_Login();
                user_Login = (User_Login)Session["UsernameOrdinary"];
                var list = (from n in db.User_Login
                            join a in db.User_Card on n.UserLogin_ID equals a.UserLogin_ID
                            into JionedEmpDept
                            from a in JionedEmpDept.DefaultIfEmpty()
                            where n.UserLogin_Name == user_Login.UserLogin_Name && n.UserLogin_PassWord == user_Login.UserLogin_PassWord
                            select new
                            {
                                用户卡片ID = a.UserCard_ID,
                                // 用户名=n.UserLogin_Name
                            });
                int Uid = 0;
                foreach (var x in list)
                {
                    Uid = x.用户卡片ID;
                }
                if (Request.Files.Count > 0)
                {
                    DirectoryInfo directory = new DirectoryInfo(Directory.GetCurrentDirectory());
                    string ext = Path.GetExtension(Request.Files[0].FileName);//directory.FullName;
                    if (Request.Files[0].ContentType == "image/jpg" || Request.Files[0].ContentType == "image/png" || Request.Files[0].ContentType == "image/gif" || Request.Files[0].ContentType == "image/jpeg")
                    {
                        Random ran = new Random();
                        string fileName = DateTime.Now.ToString("yyyyMMddhhmmss") + ran.Next(100, 1000) + Guid.NewGuid().ToString() + ext;//给文件取名
                        string fileDir = HttpContext.Server.MapPath("/Resource/Img");
                        fileDir = fileDir + "/" + fileName;
                        Request.Files[0].SaveAs(fileDir);
                        //PathImgFile = fileDir + "." + ext + "" + Request.Files[0].ContentType;
                        PathImgFile = "/Resource/Img/" + fileName;
                    }
                    else
                    {
                        PathImgFile = "/Resource/Img/20180518030030100c8fc06cd-770e-49f1-b77b-cdc54d8a0d45.jpg";
                    }
                }
                else
                {
                    PathImgFile = "/Resource/Img/20180518030030100c8fc06cd-770e-49f1-b77b-cdc54d8a0d45.jpg";
                }
                string type = collection["ArticleType_ID"].Trim() + "-" + collection["inputForum_Name"].Trim();
                int ATypeID = db.ArticleType.Where(x => x.ArticleType_Name == type).Select(x => x.ArticleType_ID).FirstOrDefault();
                var array = collection["biaoqian"].Split(',');
                string LableText = ",";
                int lableid;
                foreach (var xa in array)
                {
                    if (db.Lable.Where(x => x.Lable_Text == xa).Count() > 0)
                    {

                    }
                    else
                    {
                        Lable lable = new Lable { Lable_Text = xa, Lable_Remark = user_Login.UserLogin_Name + "新增", Lable_CreatDT = DateTime.Now };
                        db.Lable.Add(lable);
                        db.SaveChanges();
                    }
                    lableid = db.Lable.Where(x => x.Lable_Text == xa).Select(x => x.Lable_ID).FirstOrDefault();
                    LableText += lableid + ",";
                }
                db.Article.Add(new Article
                {
                    UserCard_ID = Uid,
                    Article_Title = collection["Article_Title"].ToString(),
                    Article_Abstract = collection["Article_Abstract"].ToString(),
                    Article_CreateDT = DateTime.Now,
                    Article_Img = PathImgFile,
                    Article_Content = content,
                    ArticleType_ID = ATypeID,
                    Lable_ID = LableText,
                });
                db.SaveChanges();
                return Content(type);
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }
        [HttpPost, ActionName("ArticleEdit")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Authorize(Roles = "Admin,PUTONG")]
        public ActionResult ArticleEdit(FormCollection collection)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {

                    string Article_ID = collection["Article_ID"];
                    Article article = db.Article.Where(x => x.Article_ID.ToString() == Article_ID).FirstOrDefault();
                    article.Article_Title = collection["Article_Title"];
                    article.Article_Content = collection["editor"];
                    article.Article_Abstract = collection["Article_Abstract"];
                    string type = collection["ArticleType_ID"].Trim() + "-" + collection["inputForum_Name"].Trim();
                    int ATypeID = db.ArticleType.Where(x => x.ArticleType_Name == type).Select(x => x.ArticleType_ID).FirstOrDefault();
                    var array = collection["biaoqian"].Split(',');
                    string LableText = ",";
                    int lableid;
                    foreach (var xa in array)
                    {
                        if (db.Lable.Where(x => x.Lable_Text == xa).Count() > 0)
                        {

                        }
                        else
                        {
                            Lable lable = new Lable { Lable_Text = xa, Lable_Remark = collection["Article_Title"] + "新增", Lable_CreatDT = DateTime.Now };
                            db.Lable.Add(lable);
                            db.SaveChanges();
                        }
                        lableid = db.Lable.Where(x => x.Lable_Text == xa).Select(x => x.Lable_ID).FirstOrDefault();
                        LableText += lableid + ",";
                    }
                    article.ArticleType_ID = ATypeID;
                    article.Lable_ID = LableText;
                    db.Entry(article).State = EntityState.Modified;
                    db.SaveChanges();
                    dbContextTransaction.Commit();
                    return Content("<script>alert('修改成功" + type + "需要编译？');</script>");
                }
                catch (DataException)
                {
                    dbContextTransaction.Rollback();
                    ModelState.AddModelError("", "无法保存更改。再试一次，如果问题仍然存在，请参见系统管理员。");
                    return Content("<script>alert('修改失败');</script>");
                }
            }

        }
        /// <summary>
        /// 查询标签转换
        /// </summary>
        /// <param name="Lable"></param>
        /// <returns></returns>
        public string SelectLable(string Lable)
        {
            var lables = Lable.Split(',');
            string LablesID_str = "";
            foreach (var lable in lables)
            {
                LablesID_str = LablesID_str + db.Lable.Where(x => x.Lable_ID.ToString() == lable).Select(x => x.Lable_Text).FirstOrDefault() + ",";
            }
            return LablesID_str;
        }
        #region 检测用户状态
        [HttpPost, ActionName("UserLoginImg")]
        public JsonResult UserLoginImg()
        {
            User_Login user_Login = new User_Login();
            user_Login = (User_Login)Session["UsernameOrdinary"];
            var list = (from n in db.User_Login
                        join a in db.User_Card on n.UserLogin_ID equals a.UserLogin_ID
                        into JionedEmpDept
                        from a in JionedEmpDept.DefaultIfEmpty()
                        where n.UserLogin_Name == user_Login.UserLogin_Name && n.UserLogin_PassWord == user_Login.UserLogin_PassWord
                        select new
                        {
                            权限 = a.UserCard_GmLevel,
                            状态 = n.UserLogin_State,
                            头像 = a.UserCard_ChatHeadImg,
                            用户名 = n.UserLogin_Name,
                            昵称 = a.UserCard_Nickname


                        });
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost, ActionName("UserClose")]
        public int UserClose()
        {
            Session["UsernameOrdinary"] = null;
            // return RedirectToRoute(new { Controller = "Blogs", action = "Index" });
            if (Session["UsernameOrdinary"] == null)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        [Category("检测")]
        [Description("检测用户登录状态")]
        public void CheckLoginStart()
        {
            User_Login user_Login = new User_Login();
            try
            {
                if (Session["UsernameOrdinary"] != null || Session["UsernameOrdinary"].ToString() != "")
                {
                    user_Login = (User_Login)Session["UsernameOrdinary"];
                    //string[] vs = Session["Username"].ToString().Split(',');
                    var list = (from n in db.User_Login
                                join a in db.User_Card on n.UserLogin_ID equals a.UserLogin_ID
                                into JionedEmpDept
                                from a in JionedEmpDept.DefaultIfEmpty()
                                where n.UserLogin_Name == user_Login.UserLogin_Name && n.UserLogin_PassWord == user_Login.UserLogin_PassWord
                                select new
                                {
                                    权限 = a.UserCard_GmLevel,
                                    状态 = n.UserLogin_State
                                });//内连接
                    if (list.Count() > 0)
                    {
                        foreach (var s in list)
                        {
                            if (s.状态 == true)
                            {
                                start = true;
                                //if (s.权限 == 0)
                                //{
                                //    start = true;
                                //}
                                //else
                                //{
                                //    start = false;
                                //}
                            }
                            else
                            {
                                start = false;
                            }
                        }
                    }
                }
                else
                {
                    start = false;
                }
            }
            catch (Exception e)
            {
                start = false;
            }
        }
        #endregion
    }
}