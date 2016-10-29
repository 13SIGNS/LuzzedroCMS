using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LuzzedroCMS.Domain.Properties;

namespace LuzzedroCMS.Domain.Entities
{
    [Table("LCMS_Comment")]
    public class Comment
    {
        [HiddenInput]
        public int CommentID { get; set; }

        [Display(Name = "CommentParentID", ResourceType = typeof(Resources))]
        public int ParentCommentID { get; set; }

        [Display(Name = "ArticleID", ResourceType = typeof(Resources))]
        public int ArticleID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int UserID { get; set; }

        [HiddenInput]
        public DateTime Date { get; set; }

        [Display(Name = "CommentContent", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required")]
        [MinLength(2, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveMoreChars")]
        [MaxLength(2048, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveNoMoreChars")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int Status { get; set; }
    }
}