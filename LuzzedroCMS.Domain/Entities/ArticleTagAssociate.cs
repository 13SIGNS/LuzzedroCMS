using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LuzzedroCMS.Domain.Entities
{
    [Table("LCMS_ArticleTagAssociate")]
    public class ArticleTagAssociate
    {
        public int ArticleTagAssociateID { get; set; }
        public int ArticleID { get; set; }
        public int TagID { get; set; }
        public int Status { get; set; }
    }
}