using LuzzedroCMS.Domain.Entities;
using LuzzedroCMS.WebUI.Infrastructure.Helpers;
using System.Collections.Generic;

namespace LuzzedroCMS.Models
{
    public class UsersViewModel
    {
        public IList<UserViewModel> Users { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}