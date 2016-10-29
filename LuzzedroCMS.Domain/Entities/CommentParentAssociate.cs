using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LuzzedroCMS.Domain.Entities
{
    [Table("LCMS_CommentParentAssociate")]
    public class CommentParentAssociate
    {
        public int CommentParentAssociateID { get; set; }
        public Comment CommentChild { get; set; }
        public Comment CommentParent { get; set; }
    }
}