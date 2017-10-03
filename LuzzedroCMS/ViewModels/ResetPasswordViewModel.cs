using LuzzedroCMS.Domain.Infrastructure.Concrete;
using LuzzedroCMS.WebUI.Properties;
using System.ComponentModel.DataAnnotations;

namespace LuzzedroCMS.Models
{
    public class ResetPasswordViewModel
    {
        private string password;

        [Display(Name = "InventPassword", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required")]
        [MinLength(7, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveMoreChars")]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveNoMoreChars")]
        [DataType(DataType.Password)]
        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = new TextBuilder().GetHash(value);
            }
        }
    }
}