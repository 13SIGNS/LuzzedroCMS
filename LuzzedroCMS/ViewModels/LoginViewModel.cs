using LuzzedroCMS.Domain.Infrastructure.Concrete;
using LuzzedroCMS.WebUI.Properties;
using System.ComponentModel.DataAnnotations;

namespace LuzzedroCMS.Models
{
    public class LoginViewModel
    {
        private string loginPassword { get; set; }

        public bool IsFacebookConnected { get; set; }

        public bool IsGoogleConnected { get; set; }

        [Display(Name = "EnterLoginEmail", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required")]
        [MinLength(7, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveMoreChars")]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveNoMoreChars")]
        public string LoginEmail { get; set; }

        [Display(Name = "EnterPassword", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required")]
        [MinLength(7, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveMoreChars")]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveNoMoreChars")]
        [DataType(DataType.Password)]
        public string LoginPassword
        {
            get
            {
                return loginPassword;
            }

            set
            {
                loginPassword = new TextBuilder().GetHash(value);
            }
        }

        public string ContentExternalUrl { get; set; }

        public string ImageName { get; set; }

        public string UserName { get; set; }

        public bool IsLogged { get; set; }
    }
}