using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LuzzedroCMS.Domain.Entities
{
    [Table("LCMS_BookmarkUserArticleAssociate")]
    public class BookmarkUserArticleAssociate
    {
        public int BookmarkUserArticleAssociateID { get; set; }
        public int UserID { get; set; }
        public int ArticleID { get; set; }
        public DateTime Date { get; set; }
        public int Status { get; set; }
    }
}