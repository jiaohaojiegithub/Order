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
using System.IO;
#endregion
namespace Order.Controllers
{
   
    [Category("UserCard表")]
    [Description("UserCard表-用户详细信息")]
    public class User_CardController : Controller
    {
        DBMYBLOGContext db = new DBMYBLOGContext();
        // GET: User_Card
        public ActionResult Index()
        {
            return View();
        }
        // GET: User_Card/Details/5
        [Authorize(Roles = "Admin,PUTONG")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_Card user_Card = db.User_Card.Find(id);
            if (user_Card == null)
            {
                return HttpNotFound();
            }
            return View(user_Card);
        }
        public int? UserCardID;
        [Authorize(Roles = "Admin,PUTONG")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_Card user_Card = new User_Card();// db.User_Card.Where(x => x.UserLogin_ID == id);
            foreach (User_Card a in db.User_Card.Where(x => x.UserLogin_ID == id))
            {
                user_Card = a;
                UserCardID = user_Card.UserCard_ID;
            }
            if (user_Card == null)
            {
                return HttpNotFound();
            }
            
            return View(user_Card);
        }
        /// <summary>
        /// 修改用户资料
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost, ActionName("User_CardEdit")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,PUTONG")]
        public ActionResult User_CardEdit(FormCollection collection)
        {
            string UserCard_Nickname = collection["UserCard_Nickname"].ToString();
            string Sex = collection["Sex"].ToString();
            string UserCard_Remark = collection["UserCard_Remark"].ToString();
            string UserCard_MobilPhone = collection["UserCard_MobilPhone"].ToString();
            int userCard_ID = int.Parse(collection["UserCard_ID"].Trim());
            DateTime UserCard_Birthday = DateTime.Parse(collection["UserCard_Birthday"]);
            string PathImgFile = "";
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

            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    /*
                    User_Card user_Card = new User_Card
                    {
                        UserCard_ID = userCard_ID,
                        UserCard_ChatHeadImg = PathImgFile,
                        UserCard_Birthday = UserCard_Birthday,
                        //UserCard_GmLevel = 1,//普通用户
                        UserCard_MobilPhone = UserCard_MobilPhone,
                        UserCard_Nickname = UserCard_Nickname,
                        UserCard_Sex = Sex,
                        UserCard_Remark = UserCard_Remark

                    };
                    db.Entry(user_Card).State = EntityState.Modified;
                    */
                    User_Card user_Card = db.User_Card.Where(x => x.UserCard_ID == userCard_ID).FirstOrDefault<User_Card>();
                    user_Card.UserCard_Nickname = UserCard_Nickname;
                    user_Card.UserCard_Birthday = UserCard_Birthday;
                    user_Card.UserCard_ChatHeadImg = PathImgFile;
                    user_Card.UserCard_MobilPhone = UserCard_MobilPhone;
                    user_Card.UserCard_Remark = UserCard_Remark;
                    user_Card.UserCard_Sex = Sex;
                    db.Entry(user_Card).State = EntityState.Modified;
                    db.SaveChanges();
                    dbContextTransaction.Commit();
                    return Content("<script>alert('修改成功');</script>");
                }
                catch (DataException)
                {
                    dbContextTransaction.Rollback();
                    ModelState.AddModelError("", "无法保存更改。再试一次，如果问题仍然存在，请参见系统管理员。");
                    return Content("<script>alert('修改失败');</script>");
                }
            }

            //return Content(collection["UserCard_ID"]);
        }
        [HttpGet]
        public JsonResult User_CardInfoAll()
        {
           // List<User_Card> user_Cards = db.User_Card.Include(u => u.User_Login).ToList();
            List<User_Card> user_Cards = db.User_Card.ToList<User_Card>();
            var total = user_Cards.Count;
            var rows = user_Cards.ToList();
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }
        #region 实例
        /*
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
         */
        #endregion
    }
}