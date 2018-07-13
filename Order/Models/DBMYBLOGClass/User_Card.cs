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
    public class User_Card
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//数据自增长
        public int UserCard_ID { get; set; }
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserLogin_ID { get; set; }
        [Required(ErrorMessage = "请为你的账户添加一个昵称!")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "昵称长度限制为{2}至{1}个字符")]
        public string UserCard_Nickname { get; set; }
        [Required]
        public int UserCard_GmLevel { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime UserCard_CreatDt { get; set; }
        [MaxLength(80, ErrorMessage = "备注描述字符请限制在80字符以内")]
        public string UserCard_Remark { get; set; }
        public DateTime UserCard_Birthday { get; set; }
        [Required(ErrorMessage = "请选择你的性别")]
        public string UserCard_Sex { get; set; }
        [DataType(DataType.PhoneNumber, ErrorMessage = "请填写正确的手机号码")]
        public string UserCard_MobilPhone { get; set; }
        [DataType(DataType.ImageUrl)]
        public string UserCard_ChatHeadImg { get; set; }

        public virtual User_Login User_Login { get; set; }
    }
}