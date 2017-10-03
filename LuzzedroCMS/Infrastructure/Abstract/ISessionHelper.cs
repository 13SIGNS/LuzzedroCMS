using System.Drawing;
using System.Web;
using System.Web.Mvc;

namespace LuzzedroCMS.Infrastructure.Abstract
{
    public interface ISessionHelper
    {
        Controller Controller { get; set; }
        void SetSessionUserData();
        void RemoveSessionUserData();
        string UserEmail { get; set; }
        string UserNick { get; set; }
        string UserPhotoUrl { get; set; }
        bool IsLogged { get; }
    }
}