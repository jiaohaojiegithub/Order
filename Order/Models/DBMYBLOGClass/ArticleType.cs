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
    [Category("数据下级属性表")]
    [Description("分类列表-用于定义文章的属性类别")]
    public class ArticleType
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ArticleType_ID { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "数据名必须为{2}到{1}个字符")]
        public string ArticleType_Name { get; set; }
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Forum_ID { get; set; }
    }
}