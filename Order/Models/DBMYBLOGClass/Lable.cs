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
    public class Lable
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Lable_ID { get; set; }
        [Required(ErrorMessage = "标签名不能为空")]
        public string Lable_Text { get; set; }
        [Required(ErrorMessage = "标签描述")]
        [MaxLength(150, ErrorMessage = "限制描述字符位150")]
        public string Lable_Remark { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Lable_CreatDT { get; set; }
    }
}