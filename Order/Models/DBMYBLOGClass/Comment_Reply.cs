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
    [Category("评论回复表")]
    [Description("用于对某评论发表建议")]
    public class Comment_Reply
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Comment_Reply_ID { get; set; }
        public int Comment_ID { get; set; }
        public int UserCard_ID { get; set; }
        [MaxLength(500, ErrorMessage = "字符请限制在500字符以内")]
        public string Comment_Reply_Contene { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Comment_Reply_CreatDT { get; set; }
    }
}