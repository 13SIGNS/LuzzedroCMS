using LuzzedroCMS.Domain.Entities;
using LuzzedroCMS.WebUI.ViewModels;

namespace LuzzedroCMS.Models
{
    public class UserViewModel
    {
        public User User { get; set; }
        public ChangePasswordViewModel changePasswordViewModel { get; set; }
        public string PhotoUrlPath { get; set; }
    }
}