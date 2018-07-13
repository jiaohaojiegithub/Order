using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
#region ADDUsing
using Order.Models;
using Order.Models.DBMYBLOGClass;
using Order.Controllers;
#endregion
namespace Order.Controllers
{
    public class AdminController : Controller
    {
        DBMYBLOGContext db = new DBMYBLOGContext();
        public bool start = false;
        #region 视图
        // GET: Admin
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            CheckLoginStart();
            if (start)
                return View();
            else
            {
                //return Content("<script>window.open('/login/Index', '_self');</script>");
                return RedirectToRoute(new { Controller = "LoginOrRegister", action = "Index" });
                //User_Login user_Login = (User_Login)Session["Username"];
                //return Content(user_Login.UserLogin_Name);
            }
            //return View();
        }
        /// <summary>
        /// 测试页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Test()
        {
            return PartialView();
        }

        /// <summary>
        /// UserLogin表列表信息
        /// </summary>
        /// <returns></returns>
        [Category("UserLogin表")]
        [Description("UserLogin表-列出用户信息")]
        [Authorize(Roles = "Admin")]
        public ActionResult UserLogin()
        {
            return PartialView(db.User_Login.ToList());
        }
        /// <summary>
        /// 用户新增界面
        /// </summary>
        /// <returns></returns>
        [Category("UserLogin表")]
        [Description("注册,添加用户")]
        [Authorize(Roles = "Admin")]
        public ActionResult UserLoginAdd()
        {
            return PartialView();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult UserCardInfo()
        {
            return PartialView();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ArticleADD()
        {
            return PartialView();
        }
        #endregion
        #region 方法
        [HttpPost, ActionName("ArticleADD")]//
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ArticleADD(FormCollection collection)
        {
            var content = collection["editor"];
            string PathImgFile = "";
            User_Login user_Login = new User_Login();
            user_Login = (User_Login)Session["Username"];
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
                    PathImgFile = "/Resource/Img/2018051802195072700000000-0000-0000-0000-000000000000.jpg";
                }
            }
            else
            {
                PathImgFile = "/Resource/Img/2018051802195072700000000-0000-0000-0000-000000000000.jpg";
            }
            //string type = collection["inputForum_Name"] + "-" + collection["ArticleType_ID"];
string type = collection["ArticleType_ID"]+ "-" +collection["inputForum_Name"];
            int ATypeID = db.ArticleType.Where(x => x.ArticleType_Name == type).Select(x => x.ArticleType_ID).FirstOrDefault();
            string LableText = ",";
            int lableid;
            for (int i = 1; i <= 5; i++)
            {
                if (collection["biaoqian" + i].ToString() != "" && collection["biaoqian" + i].ToString() != "标签")
                {
                    if (db.Lable.Where(x => x.Lable_Text == collection["biaoqian" + i].ToString()).Count() > 0)
                    {

                    }
                    else
                    {
                        Lable lable = new Lable { Lable_Text = collection["biaoqian" + i].ToString(),Lable_Remark= user_Login.UserLogin_Name+"新增",Lable_CreatDT=DateTime.Now };
                        db.Lable.Add(lable);
                        db.SaveChanges();
                    }
                    lableid = db.Lable.Where(x => x.Lable_Text == collection["biaoqian" + i].ToString()).Select(x => x.Lable_ID).FirstOrDefault();
                    LableText +=lableid+"," ;
                }
               
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
                Lable_ID= LableText,
            });
            db.SaveChanges();
            return Content(content);
        }
        //[HttpPost]
        //public ActionResult UsersADD(HttpContext context)
        //{
        //    context.Response.ContentType = "text/plain";
        //    string name = context.Request["UserLogin_Name"].ToString();
        //    return Content(name);
        //}
        [Category("用户添加")]
        [Description("UserLogin表，User_Card表")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UsersADD(FormCollection collection)
        {
            string sat = "";
            string name = collection["UserLogin_Name"].ToString();
            string password = collection["UserLogin_PassWord"].ToString();
            string UserCard_Nickname = collection["UserCard_Nickname"].ToString();
            string UserCard_GmLevel = collection["UserCard_GmLevel"].ToString();
            string Sex = collection["Sex"].ToString();
            string UserCard_Remark = collection["UserCard_Remark"].ToString();
            string UserCard_MobilPhone = collection["UserCard_MobilPhone"].ToString();
            DateTime UserCard_Birthday = DateTime.Parse(collection["UserCard_Birthday"]);
            string PathImgFile = "";
            if (name != null && password != null)
            {
                if (db.User_Login.Where(x => x.UserLogin_Name == name).Count() == 0)
                {
                    db.User_Login.Add(new User_Login
                    {
                        UserLogin_Name = name,
                        UserLogin_PassWord = password,
                        UserLogin_State = true
                    });
                    db.SaveChanges();
                    var UserLoginInfo = db.User_Login.Where(x => x.UserLogin_Name == name).ToList();
                    int id = 0;
                    int GmLevel = 1;
                    switch (UserCard_GmLevel)
                    {
                        case "管理员": GmLevel = 0; break;
                        default: GmLevel = 1; break;
                    }
                    foreach (var x in UserLoginInfo)
                    {
                        id = x.UserLogin_ID;
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
                    db.User_Card.Add(new User_Card
                    {
                        UserLogin_ID = id,
                        UserCard_GmLevel = GmLevel,
                        UserCard_Birthday = UserCard_Birthday,
                        UserCard_ChatHeadImg = PathImgFile,
                        UserCard_MobilPhone = UserCard_MobilPhone,
                        UserCard_Nickname = UserCard_Nickname,
                        UserCard_Sex = Sex,
                        UserCard_Remark = UserCard_Remark,
                        UserCard_CreatDt = DateTime.Now
                    });
                    db.SaveChanges();
                    sat = "注册成功";
                }
                else
                {
                    sat = "用户名已存在";
                }

            }

            //foreach (HttpPostedFile f in Request.Files)
            //{
            //   name=name+ f.FileName.ToString();
            //}

            //return Content(name+"\n"+password+"\n"+UserCard_Nickname+"\n"+UserCard_GmLevel+"\n"+Sex+"<br/>"+ UserCard_Remark+"<br/>"+UserCard_Birthday.ToShortDateString()+"<br/>"+ UserCard_MobilPhone+ PathImgFile);
            return Content(sat);
        }
        [Category("图片上传")]
        [Description("UserLoginAdd动作中图片上传地址")]
        [HttpPost]
        public void UploadSerivcePhoto(IEnumerable<HttpPostedFileBase> FilesInput)
        {
            /*
             我有同样的问题。检查请求。文件属性如@Michael所提到的，它应该包含几个文件。当你检查的时候，你也应该过滤掉那些空着的名字:
             
            List<HttpPostedFileBase> allFiles = Enumerable.Range(0, Request.Files.Count)
                                               .Select(x => Request.Files[x])
                                               .Where(x => !string.IsNullOrEmpty(x.FileName))
                                              .ToList();
             */

        }
        #region 图片处理解决方案
        /*
         [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [UIExceptionResult]
        public ActionResult attachment_create(create model)
        {
            if (ModelState.IsValid)
            {
               
                    string uploadPath = Server.MapPath(string.Format("~\\upload\\{0}\\", DateTime.Now.ToString("yyyyMMdd")));
                    string savePath = string.Format("/upload/{0}/", DateTime.Now.ToString("yyyyMMdd"));
                    if (Directory.Exists(uploadPath) == false)
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    if (model.attch != null && model.attch.Count() > 0)
                    {
                        for (int i = 0; i < model.attch.Count(); i++)
                        {
                            var _file = model.attch[i];
                            string _imageName = DateTime.Now.Ticks.ToString() + System.IO.Path.GetExtension(_file.FileName);
                            var path = uploadPath + _imageName;
                            //保存
                            _file.SaveAs(path);
                        }

                    }
            return View(model);
        }
         */
        /*
         * 一,Form包含文件类（单选文件）
         <form id="ImgForm" method="POST" enctype="multipart/form-data" name="ImgForm" action="/From/SubmitForm">
               <input type="file" name="fileData" >
               <br />
               <input type="text" name="name" />
               <br />
               <input type="password" name="password" />
               <br />
               <input type="submit" value="上传" />
           </form>
           [HttpPost]
        public void SubmitForm(HttpPostedFileBase fileData, FormCollection collection)
        {
            string name = collection["name"];
            string passowrd = collection["password"];
            string image = fileData.FileName.ToString();
            string imageGuid = Guid.NewGuid().ToString() + ".jpg";
            string path = System.Web.HttpContext.Current.Server.MapPath("~/QRimage/") + imageGuid;//只是用于演示简单的上传删除方法
            fileData.SaveAs(path);
            string deletePath = "http://wx115.cnpsim.com/QRimage/" + imageGuid;
            var img = new FileInfo(path);
            if (img.Exists) img.Delete();
        }
        *二,Form包含文件类（多选文件）
        * <div>
            <form id="ImgForm" method="POST" enctype="multipart/form-data" name="ImgForm" action="/From/SubmitForm">
                <input type="file" name="fileData" multiple />
                <br />
                <input type="text" name="name" />
                <br />
                <input type="password" name="password" />
                <br />
                <input type="submit" value="上传" />
            </form>
        </div>
        [HttpPost]
        public void SubmitForm(HttpPostedFileBase[] fileData, FormCollection collection)
        {
            string name = collection["name"];
            string passowrd = collection["password"];
            foreach (var file in fileData)
            {
                string image = file.FileName.ToString();
                string imageGuid = Guid.NewGuid().ToString() + ".jpg";
                string path = System.Web.HttpContext.Current.Server.MapPath("~/QRimage/") + imageGuid;//只是用于演示简单的上传删除方法
                file.SaveAs(path);
                string deletePath = "http://wx115.cnpsim.com/QRimage/" + imageGuid;
                var img = new FileInfo(path);
                if (img.Exists) img.Delete();
            }
        }
        *Form包含文件类（多选文件多Input file）
        <div>
            <form id="ImgForm" method="POST" enctype="multipart/form-data" name="ImgForm" action="/From/SubmitForm">
                <input type="file" name="fileData" multiple />
                <br />
                <input type="file" name="fileData" multiple />
                <br />
                <input type="file" name="fileData" multiple />
                <br />
                <input type="text" name="name" />
                <br />
                <input type="password" name="password" />
                <br />
                <input type="submit" value="上传" />
            </form>
        </div>
         [HttpPost]
        public void SubmitForm(FormCollection collection)
        {
            string name = collection["name"];
            string passowrd = collection["password"];
            for(int i=0;i<Request.Files.Count;i++)
            {
                HttpPostedFileBase f = Request.Files[i];
                string nwame = f.FileName.ToString();
            }
            
        }
        */
        #endregion
        #endregion
        #region 登录-用户验证等
        [HttpPost, ActionName("UserLoginImg")]
        public JsonResult UserLoginImg()
        {
            User_Login user_Login = new User_Login();
            user_Login = (User_Login)Session["Username"];
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
        [Category("检测")]
        [Description("检测用户登录状态")]
        public void startCheck()
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
        }
        public void CheckLoginStart()
        {
            User_Login user_Login = new User_Login();
            try
            {
                if (Session["Username"] != null || Session["Username"].ToString() != "")
                {
                    user_Login = (User_Login)Session["Username"];
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
                                if (s.权限 == 0)
                                {
                                    start = true;
                                }
                                else
                                {
                                    start = false;
                                }
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