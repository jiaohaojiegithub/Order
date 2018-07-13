using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#region UsingADD
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
#endregion
namespace Order.Models.DBMYBLOGClass
{
    [Category("数据分类表")]
    [Description("分类列表-用于划分数据类别")]
    public class Forum
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Forum_ID { get; set; }
        [Required(ErrorMessage ="数据名不能为空")]
        [StringLength(10,MinimumLength =1,ErrorMessage = "数据名必须为{2}到{1}个字符")]
        public string Forum_Name { get; set; }
    }
}