using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using LuzzedroCMS.Domain.Properties;
using System.Collections.Generic;

namespace LuzzedroCMS.Domain.Entities
{
    [Table("LCMS_Article")]
    public class Article
    {
        [HiddenInput(DisplayValue = false)]
        public int ArticleID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int UserID { get; set; }

        [Display(Name = "CategoryID", ResourceType = typeof(Resources))]
        public virtual Category Category { get; set; }

        public virtual IList<Comment> Comments { get; set; }

        public virtual IList<Tag> Tags { get; set; }

        public virtual Photo Photo { get; set; }

        [Display(Name = "MainPhotoName", ResourceType = typeof(Resources))]
        [MinLength(5, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveMoreChars")]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveNoMoreChars")]
        public string ImageName { get; set; }


        [Display(Name = "MainPhotoDesc", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required")]
        [MinLength(5, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveMoreChars")]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveNoMoreChars")]
        public string ImageDesc { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime DateAdd { get; set; }

        [Display(Name = "DateArticleDisplayed", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required")]
        [DataType(DataType.DateTime)]
        public DateTime DatePub { get; set; }

        [Display(Name = "DateArticleDisplayedTo", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required")]
        [DataType(DataType.DateTime)]
        public DateTime DateExp { get; set; }

        [Display(Name = "Title", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required")]
        [MinLength(5, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveMoreChars")]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveNoMoreChars")]
        public string Title { get; set; }

        [Display(Name = "Url", ResourceType = typeof(Resources))]
        [HiddenInput(DisplayValue = false)]
        public string Url { get; set; }

        [Display(Name = "Lead", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required")]
        [MinLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveMoreChars")]
        [MaxLength(1000, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveNoMoreChars")]
        [DataType(DataType.MultilineText)]
        public string Lead { get; set; }

        [AllowHtml]
        [Display(Name = "Content", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required")]
        [MinLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveMoreChars")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Display(Name = "Sources", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required")]
        [MaxLength(2000, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveNoMoreChars")]
        [DataType(DataType.MultilineText)]
        public string Source { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int Status { get; set; }
    }
}