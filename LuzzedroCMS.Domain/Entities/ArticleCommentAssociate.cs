using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LuzzedroCMS.Domain.Entities
{
    [Table("LCMS_ArticleCommentAssociate")]
    public class ArticleCommentAssociate
    {
        public int ArticleCommentAssociateID { get; set; }
        public int ArticleID { get; set; }
        public int CommentID { get; set; }
        public int Status { get; set; }
    }
}