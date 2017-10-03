using LuzzedroCMS.WebUI.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LuzzedroCMS.WebUI.ViewModels
{
    public class NickViewModel
    {
        [Display(Name = "Nick", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required")]
        [MinLength(3, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveMoreChars")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveNoMoreChars")]
        public string Nick { get; set; }
    }
}