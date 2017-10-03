using LuzzedroCMS.Domain.Abstract;
using LuzzedroCMS.Domain.Entities;
using LuzzedroCMS.Models;
using System;
using System.Web;
using System.Web.Mvc;

namespace LuzzedroCMS.Infrastructure.Abstract
{
    public interface IAccount
    {
        void Logout(Controller controller);
        void RegisterUserByEmail(string email);
        bool RegisterAndLogByEmail(string email, int source, Controller controller);
        bool IsUserRegistered(string email);
        void SaveUserInSession(Controller controller);
        bool SetUserLogged(string email, string password, IUserRepository repo, Controller controller);
        void SetUserLoggedAbsolutely(string email, IUserRepository repo, Controller controller);
        UserTemp SetUserTemp(IUserRepository repo, RegisterViewModel model, string randomToken);
        string GetInviteUserEmailContent(RegisterViewModel model, string action);
        string GetInviteUserEmailSubject();
        string GetRemindUserEmailSubject();
        string GetRemindUserEmailContent(RemindViewModel model, string action, User user);
        bool SendInviteUserEmail(RegisterViewModel model, string action);
        bool SendRemindUserEmail(RemindViewModel model, string action, User user);
        bool IsSetUser(User userByEmail, User userByNick, RegisterViewModel model);
        string GetUniqueToken(string token, IUserRepository repo);
        UserToken GetNewUserToken(IUserRepository repo, User user, string randomToken);
        bool IsTokenValid(DateTime date);
    }
}