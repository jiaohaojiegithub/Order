using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
#region ADDUsing
using Order.Models;
using Order.Models.DBMYBLOGClass;
using Order.Controllers;
using System.ComponentModel;
#endregion
namespace Order.Controllers
{
    public class BlogsController : Controller
    {
        DBMYBLOGContext db = new DBMYBLOGContext();
        public bool start = false;
        // GET: Blogs
        public ActionResult Index()
        {
            CheckLoginStart();
            if (start)
            {
                ViewBag.stata = 1;
            }
            else
            {
                ViewBag.stata = 2;
            }
            return View();
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
                                昵称 = a.UserCard_Nickname,
                                ID = n.UserLogin_ID,
                                用户详细ID = a.UserCard_ID

                            });
                return Json(list.ToList(), JsonRequestBehavior.AllowGet);
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