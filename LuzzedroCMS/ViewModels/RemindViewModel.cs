using LuzzedroCMS.WebUI.Properties;
using System.ComponentModel.DataAnnotations;

namespace LuzzedroCMS.Models
{
    public class RemindViewModel
    {
        [Display(Name = "EnterLoginEmail", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required")]
        [MinLength(7, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveMoreChars")]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveNoMoreChars")]
        public string Email { get; set; }
    }
}