using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LuzzedroCMS.Domain.Entities
{
    [Table("LCMS_ArticleCategoryAssociate")]
    public class ArticleCategoryAssociate
    {
        public int ArticleCategoryAssociateID { get; set; }
        public int ArticleID { get; set; }
        public int CategoryID { get; set; }
        public int Status { get; set; }
    }
}