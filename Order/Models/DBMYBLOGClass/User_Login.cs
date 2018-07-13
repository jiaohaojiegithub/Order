using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#region UsingADD
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#endregion
namespace Order.Models.DBMYBLOGClass
{
    public class User_Login
    {
        public User_Login()
        {
            UserCard = new HashSet<User_Card>();
        }
        [Required]
        [Display(Name = "用户ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//数据自增长
        public int UserLogin_ID { get; set; }
        [Required(ErrorMessage = "用户名不能为空")]
        [Display(Name = "用户名")]
        [RegularExpression(@"^[A-Za-z0-9\u4e00-\u9fa5]+$", ErrorMessage = "用户名只能由数字,汉字,字母组成")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "用户名必须为{2}到{1}个字符")]
        public string UserLogin_Name { get; set; }
        [Required(ErrorMessage = "为了您的账户安全，请设置密码")]
        [Display(Name = "密码")]
        [StringLength(18, MinimumLength = 6, ErrorMessage = "密码必须为{2}到18个字符")]
        [DataType(DataType.Password)]
        public string UserLogin_PassWord { get; set; }
        [Required]
        [Display(Name = "状态")]
        public bool UserLogin_State { get; set; }
        /*
      DateTime字段类型对应的时间格式是yyyy-MM-dd HH:mm:ss.fff，3个f，精确到1毫秒(ms)，示例2014-12-0317:06:15.433。
      DateTime2(7)字段类型对应的时间格式是yyyy-MM-dd HH:mm:ss.fffffff，7个f，精确到0.1微秒(μs)，示例2014-12-0317:23:19.2880929。
      如果用SQL的日期函数进行赋值，DateTime字段类型要用GETDATE()，DateTime2字段类型要用SYSDATETIME()。
           */
        [DataType(DataType.DateTime)]
        [Display(Name = "创建日期")]
        public DateTime UserLogin_CreatDT { get; set; }
        [Required]
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //[MaxLength(32)]
        [Display(Name = "标识")]
        public Guid UserLogin_Guid { get; set; }
        public ICollection<User_Card> UserCard { get; set; }
    }
}