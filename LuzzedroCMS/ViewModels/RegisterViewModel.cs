using LuzzedroCMS.Domain.Infrastructure.Concrete;
using LuzzedroCMS.WebUI.Properties;
using System.ComponentModel.DataAnnotations;

namespace LuzzedroCMS.Models
{
    public class RegisterViewModel
    {
        private string registerPassword;

        [Display(Name = "EnterNewLoginEmail", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required")]
        [MinLength(7, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveMoreChars")]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveNoMoreChars")]
        public string RegisterEmail { get; set; }

        [Display(Name = "InventPassword", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required")]
        [MinLength(7, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveMoreChars")]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveNoMoreChars")]
        [DataType(DataType.Password)]
        public string RegisterPassword
        {
            get
            {
                return registerPassword;
            }

            set
            {
                registerPassword = new TextBuilder().GetHash(value);
            }
        }

        public bool IsLogged { get; set; }
    }
}