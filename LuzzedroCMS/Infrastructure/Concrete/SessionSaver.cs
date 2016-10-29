using LuzzedroCMS.Domain.Abstract;
using LuzzedroCMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuzzedroCMS.Infrastructure.Concrete
{
    public class SessionSaver
    {
        private IUserRepository repoUser;
        private User user;

        public SessionSaver(IUserRepository userRepo)
        {
            repoUser = userRepo;
            user = repoUser.UserByEmail(System.Web.HttpContext.Current.User.Identity.Name);
        }

        public void SetSessionUserData()
        {
            if (user != null)
            {
                HttpContext.Current.Session["User.Name"] = user.Nick;
                HttpContext.Current.Session["User.PhotoUrl"] = user.PhotoUrl;
            }
        }
    }
}