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
    [Table("LCMS_Photo")]
    public class Photo
    {
        [HiddenInput(DisplayValue = false)]
        public int PhotoID { get; set; }

        public DateTime Date { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Name { get; set; }

        [Display(Name = "MainPhotoDesc", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required")]
        [MinLength(5, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveMoreChars")]
        [MaxLength(2048, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveNoMoreChars")]
        public string Desc { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int Status { get; set; }
    }
}