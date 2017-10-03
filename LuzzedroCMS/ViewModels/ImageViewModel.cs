using LuzzedroCMS.WebUI.Properties;
using System.ComponentModel.DataAnnotations;

namespace LuzzedroCMS.Models
{
    public class ImageViewModel
    {
        [Display(Name = "EnterSequence", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required")]
        [MinLength(7, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveMoreChars")]
        [MaxLength(280, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveNoMoreChars")]
        public string ImageDesc { get; set; }
    }
}