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
    [Category("文章表")]
    [Description("功能列表-文章")]
    public class Article
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Article_ID { get; set; }
        [Required(ErrorMessage ="请输入文章作者")]
        public int UserCard_ID { get; set; }
        public string Article_Title { get; set; }
        [DataType(DataType.ImageUrl)]
        public string Article_Img { get; set; }
        [DataType(DataType.Text)]
        public string Article_Abstract { get; set; }
        [DataType(DataType.Html)]
        public string Article_Content { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Article_CreateDT { get; set; }
        [Required]
        public int ArticleType_ID { get; set; }
        [Required]
        public string Lable_ID { get; set; }

    }
}