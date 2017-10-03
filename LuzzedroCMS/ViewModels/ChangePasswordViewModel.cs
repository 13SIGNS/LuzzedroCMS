using LuzzedroCMS.Domain.Infrastructure.Concrete;
using LuzzedroCMS.Domain.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LuzzedroCMS.WebUI.ViewModels
{
    public class ChangePasswordViewModel
    {
        private string oldPassword;
        private string newPassword;

        [Display(Name = "ExistingPassword", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required")]
        [MinLength(7, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveMoreChars")]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveNoMoreChars")]
        [DataType(DataType.Password)]
        public string OldPassword
        {
            get
            {
                return oldPassword;
            }

            set
            {
                oldPassword = new TextBuilder().GetHash(value);
            }
        }

        [Display(Name = "InventPassword", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required")]
        [MinLength(7, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveMoreChars")]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FieldMustHaveNoMoreChars")]
        [DataType(DataType.Password)]
        public string NewPassword
        {
            get
            {
                return newPassword;
            }

            set
            {
                newPassword = new TextBuilder().GetHash(value);
            }
        }
    }
}