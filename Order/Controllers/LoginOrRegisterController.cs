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
    public class LoginOrRegisterController : Controller
    {
        CheckRole roles = new CheckRole();
        DBMYBLOGContext db = new DBMYBLOGContext();
        HttpCookie cookie = new HttpCookie("User");
        public bool start = false;
        [Category("登录")]
        [Description("用户登录-管理员")]
        // GET: LoginOrRegister
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost, ActionName("LoginAdmin")]
        public int LoginAdmin(string username, string password)
        {
            var state = 0;
            if (db.User_Login.Where(x => x.UserLogin_Name == username).ToList().Count > 0)
            {
                List<User_Login> user = db.User_Login.Where(x => x.UserLogin_Name == username).ToList();
                if (user.Select(x => x.UserLogin_PassWord == password).Count() > 0)
                {

                    var list = (from n in user
                                join a in db.User_Card on n.UserLogin_ID equals a.UserLogin_ID
                                into JionedEmpDept
                                from a in JionedEmpDept.DefaultIfEmpty()
                                select new
                                {
                                    权限 = a.UserCard_GmLevel,
                                    状态 = n.UserLogin_State
                                });//内连接
                    foreach (var s in list)
                    {
                        if (s.状态 == true)
                        {
                            if (s.权限 == 0)
                                state = 1;
                            else
                                state = 3;
                        }
                        else
                            state = 4;
                    }
                    //state = 1;//登录成功
                    if (state == 1)
                    {
                        /*
                        if (Request.Cookies.Get("username") != null)//Request.Cookies["username"]!=null
                        {
                            //Cookie已经存在
                        }*/
                        //return RedirectToAction( "_operator", "operatorInfo" );
                        //User_Login user_Login = db.User_Login.Where(x => x.UserLogin_Name == username);
                        User_Login user_Login = new User_Login();
                        var user_Logins = from a in db.User_Login
                                          where a.UserLogin_Name == username && a.UserLogin_PassWord == password
                                          select a;
                        foreach (var a in user_Logins)
                        {
                            user_Login = a;
                        }
                        Session["Username"] = user_Login;
                        /*使用Cookie*/

                        //cookie.Value = "用户名";//单值
                        cookie.Values["username"] = HttpUtility.UrlEncode(username);
                        //cookie.Values["password"] = HttpUtility.UrlEncode(password);//多值
                        //cookie.Expires = DateTime.MaxValue;//过期时间
                        cookie.Expires = DateTime.Now.AddHours(1);
                        Response.Cookies.Add(cookie);
                        roles.SetRole(username, "Admin");
                    }
                    else
                    {
                        Response.Cookies["User"].Values["username"] = null;
                        Session["Username"] = null;
                        Session.RemoveAll();
                        Session.Clear();
                    }
                }
                else
                {
                    state = 2;//密码错误
                    Session.RemoveAll();
                    Session.Clear();
                }
            }
            else
            {
                state = 0;//无此用户
                Session.RemoveAll();
                Session.Clear();
            }
            return state;
        }
        /// <summary>
        /// 普通用户登陆
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost, ActionName("LoginManagementOrdinary")]
        public int LoginManagementOrdinary(string UNM, string PWD)
        {
            var state = 0;
            int i = -1;
            List<User_Login> ad = db.User_Login.ToList();
            foreach (var ax in ad)
            {
                if (ax.UserLogin_Name == UNM && ax.UserLogin_PassWord == PWD)
                {
                    i = ax.UserLogin_ID;
                }
                else
                {
                    state = 0;
                }
            }
            if (i != -1)
            {
                List<User_Login> user = db.User_Login.Where(x => x.UserLogin_ID == i).ToList();
                var list = (from n in user
                            join a in db.User_Card on n.UserLogin_ID equals a.UserLogin_ID
                            into JionedEmpDept
                            from a in JionedEmpDept.DefaultIfEmpty()
                            select new
                            {
                                权限 = a.UserCard_GmLevel,
                                状态 = n.UserLogin_State
                            });//内连接
                foreach (var s in list)
                {
                    if (s.状态 == true)
                    {
                        state = 1;
                    }
                    else
                        state = 4;
                }
                if (state == 1)
                {
                    User_Login user_Login = new User_Login();
                    var user_Logins = from a in db.User_Login
                                      where a.UserLogin_ID == i
                                      select a;
                    foreach (var a in user_Logins)
                    {
                        user_Login = a;
                    }
                    Session["UsernameOrdinary"] = user_Login;
                    roles.SetRole(UNM, "PUTONG");

                }
                else
                {
                    Session["UsernameOrdinary"] = null;
                    Session.RemoveAll();
                    Session.Clear();
                }
            }
            else
            {
                state = 0;//无此用户
                Session.RemoveAll();
                Session.Clear();
            }
            return state;
        }
        [HttpPost, ActionName("RegisterManagementOrdinary")]
        public int RegisterManagementOrdinary(string UserName, string PassWord)
        {
            var state = 0;//注册状态
            int i = -1;//用户ID
            if (UserName.Trim() != "" && PassWord.Trim() != "")
            {
                List<User_Login> ad = db.User_Login.ToList();
                foreach (var ax in ad)
                {
                    if (ax.UserLogin_Name == UserName && ax.UserLogin_PassWord == PassWord)
                    {
                        i = ax.UserLogin_ID;
                    }
                    else
                    {
                        state = 1;//用户已经存在
                    }
                }
                if (i == -1)
                {
                    using (var dbContextTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //int MAXID = ad.Max(x => x.UserLogin_ID);//db.User_Login.Max(x => x.UserLogin_ID);//db.User_Login.Where(x=>x.UserLogin_ID)
                            string PathImgFile = "/Resource/Img/20180518030030100c8fc06cd-770e-49f1-b77b-cdc54d8a0d45.jpg";
                            db.User_Login.Add(new User_Login
                            {
                                UserLogin_Name = UserName,
                                UserLogin_PassWord = PassWord,
                                UserLogin_State = true
                            });
                            db.SaveChanges();
                            int MAXID = db.User_Login.Max(x => x.UserLogin_ID);
                            //普通用户
                            db.User_Card.Add(new User_Card
                            {
                                UserLogin_ID = MAXID,
                                UserCard_GmLevel = 1,
                                UserCard_Birthday = DateTime.Now,
                                UserCard_ChatHeadImg = PathImgFile,
                                UserCard_MobilPhone = "159-0000-0001",
                                UserCard_Nickname = UserName,
                                UserCard_Sex = "未知",
                                UserCard_Remark = "新注册用户",
                                UserCard_CreatDt = DateTime.Now
                            });
                            db.SaveChanges();

                            dbContextTransaction.Commit();
                            state = 10;//注册成功
                        }
                        catch (Exception ex)
                        {
                            dbContextTransaction.Rollback();
                            state = 5;//注册失败
                        }
                    }
                }
            }
            else
            {
                state = 8;//用户名和密码不得为空
            }
            return state;
        }


        #region 原方案-废止
        [Category("检测")]
        [Description("检测用户登录状态")]
        public bool startCheck()
        {
            //string scookie = "";
            string username = "";
            if (Request.Cookies.Count > 0)
            {
                for (int i = 0; i < Request.Cookies.Count; i++)
                {
                    HttpCookie cookies = Request.Cookies[i];
                    if (cookies.Name == "User")
                    {
                        if (cookies.HasKeys)//是否有子键 
                        {
                            //scookie = "键名" + HttpUtility.UrlDecode(Request.Cookies["User"]["username"]) + HttpUtility.UrlDecode(cookies["password"]);//cookies.Values["password"]
                            //cookies.Expires = DateTime.Now.AddDays(-30);//设置过期
                            username = HttpUtility.UrlDecode(Request.Cookies["User"]["username"]);
                        }
                    }
                }
            }
            string s = "";
            if (Session["Username"] != null)//(Session["Username"] != null || username != "")//放弃Cookie
            {
                if (Session["Username"] != null)
                {
                    s = Session["Username"].ToString();
                }
                var userinfo = (from a in db.User_Login
                                join b in db.User_Card on a.UserLogin_ID equals b.UserLogin_ID
                                where a.UserLogin_Name == username //|| a.UserName == s
                                select new
                                {
                                    用户名 = a.UserLogin_Name,
                                    状态 = a.UserLogin_State,
                                    权限 = b.UserCard_GmLevel
                                });
                foreach (var str in userinfo)
                {
                    if (str.用户名 != null && str.状态 == true && str.权限 == 0)
                        start = true;
                    else
                        start = false;
                }
            }
            else
            {
                start = false;
            }
            return start;
        }
        [Category("退出登录状态")]
        [Description("检测用户登录状态,自动退出登录状态")]
        public ActionResult loginout()
        {
            if (Session["Username"] != null)
            {
                Session["Username"] = null;
            }
            if (Request.Cookies.Count > 0)
            {
                for (int i = 0; i < Request.Cookies.Count; i++)
                {
                    HttpCookie cookies = Request.Cookies[i];
                    if (cookies.Name == "User")
                    {
                        if (cookies.HasKeys)//是否有子键 
                        {
                            //scookie = "键名" + HttpUtility.UrlDecode(Request.Cookies["User"]["username"]) + HttpUtility.UrlDecode(cookies["password"]);//cookies.Values["password"]
                            //cookies.Expires = DateTime.Now.AddDays(-30);//设置过期
                            Response.Cookies["User"].Values["username"] = null;
                            cookies.Expires = DateTime.Now.AddDays(-30);
                        }
                    }
                }
            }
            return RedirectToRoute(new { Controller = "LoginOrRegister", action = "Index" });
        }
        #endregion
    }
}