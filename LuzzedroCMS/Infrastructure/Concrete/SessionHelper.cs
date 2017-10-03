using LuzzedroCMS.Domain.Abstract;
using LuzzedroCMS.Domain.Entities;
using LuzzedroCMS.Infrastructure.Abstract;
using System.Web;
using System.Web.Mvc;

namespace LuzzedroCMS.Infrastructure.Concrete
{
    public class SessionHelper : ISessionHelper
    {
        private IUserRepository repoUser;
        private User user;
        private const string NULL_IMAGE = "null.png";

        public Controller Controller { get; set; }

        public SessionHelper(IUserRepository userRepo)
        {
            repoUser = userRepo;
        }

        public string UserEmail
        {
            get
            {
                UpdateSessionUserData();
                return Controller.Session["User.Name"] != null ? Controller.Session["User.Name"].ToString() : string.Empty;
            }
            set
            {
                Controller.Session["User.Name"] = value;
            }
        }

        public string UserPhotoUrl
        {
            get
            {
                UpdateSessionUserData();
                return Controller.Session["User.PhotoUrl"] != null ? Controller.Session["User.PhotoUrl"].ToString() : NULL_IMAGE;
            }
            set
            {
                Controller.Session["User.PhotoUrl"] = value;
            }
        }

        public string UserNick
        {
            get
            {
                UpdateSessionUserData();
                return Controller.Session["User.Nick"] != null ? Controller.Session["User.Nick"].ToString() : string.Empty;
            }
            set
            {
                Controller.Session["User.Nick"] = value;
            }
        }

        public void SetSessionUserData()
        {
            user = repoUser.User(email: Controller.User.Identity.Name);
            if (user != null)
            {
                UserNick = user.Nick;
                UserPhotoUrl = user.PhotoUrl;
                UserEmail = user.Email;
            }
        }

        private void UpdateSessionUserData()
        {
            if (
               IsLogged &&
               (Controller.Session["User.Name"] == null ||
               string.IsNullOrEmpty(Controller.Session["User.Name"].ToString())))
            {
                SetSessionUserData();
            }
        }

        public void RemoveSessionUserData()
        {
            Controller.Session.Remove("User.Nick");
            Controller.Session.Remove("User.Name");
            Controller.Session.Remove("User.PhotoUrl");
        }

        public bool IsLogged
        {
            get
            {
                return Controller != null && Controller.User != null && !string.IsNullOrEmpty(Controller.User.Identity.Name);
            }
        }
    }
}