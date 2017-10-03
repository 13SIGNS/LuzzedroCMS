using LuzzedroCMS.Domain.Abstract;
using LuzzedroCMS.Domain.Entities;
using LuzzedroCMS.Domain.Infrastructure.Abstract;
using LuzzedroCMS.Infrastructure.Abstract;
using LuzzedroCMS.Infrastructure.Authorization;
using LuzzedroCMS.Models;
using LuzzedroCMS.WebUI.Infrastructure.Static;
using LuzzedroCMS.WebUI.Properties;
using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LuzzedroCMS.Infrastructure.Concrete
{
    public class Account : IAccount
    {
        private MyMembershipProvider myMembershipProvider;
        private IEmailSender sender;
        private IConfigurationKeyRepository repoConfig;
        private IUserRepository repoUser;
        private ITextBuilder textBuilder;
        private ISessionHelper repoSession;

        public Account(IEmailSender emailSender,
            IConfigurationKeyRepository configRepo,
            ITextBuilder txtBuilder,
            IUserRepository userRepo,
            ISessionHelper sessionRepo)
        {
            myMembershipProvider = new MyMembershipProvider();
            sender = emailSender;
            repoConfig = configRepo;
            textBuilder = txtBuilder;
            repoUser = userRepo;
            repoSession = sessionRepo;
        }

        public Account()
        {
        }

        public void Logout(Controller controller)
        {
            repoSession.Controller = controller;
            FormsAuthentication.SignOut();
            repoSession.RemoveSessionUserData();
        }

        public void RegisterUserByEmail(string email)
        {

        }

        public void SaveUserInSession(Controller controller)
        {
            repoSession.Controller = controller;
            repoSession.SetSessionUserData();
        }

        public bool IsUserRegistered(string email)
        {
            return repoUser.User(email: email) != null;
        }

        public bool RegisterAndLogByEmail(string email, int source, Controller controller)
        {
            if (!IsUserRegistered(email))
            {
                string pass = textBuilder.GetRandomString();
                string randomToken = GetUniqueToken(textBuilder.GetRandomString(), repoUser);
                SetUserTemp(repoUser, new RegisterViewModel
                {
                    RegisterEmail = email,
                    RegisterPassword = pass
                }, randomToken);
                User user = repoUser.TransferUserTemp(randomToken, source);
                if (user == null || !SetUserLogged(email, user.Password, repoUser, controller))
                {
                    return false;
                }
            }
            else
            {
                SetUserLoggedAbsolutely(email, repoUser, controller);
            }
            return true;
        }

        public bool SetUserLogged(string email, string password, IUserRepository repo, Controller controller)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password) && myMembershipProvider.ValidateUser(email, password))
            {
                FormsAuthentication.SetAuthCookie(email, true);
                SaveUserInSession(controller);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetUserLoggedAbsolutely(string email, IUserRepository repo, Controller controller)
        {
            if (!string.IsNullOrEmpty(email))
            {
                FormsAuthentication.SetAuthCookie(email, true);
                SaveUserInSession(controller);
            }
        }

        public UserTemp SetUserTemp(IUserRepository repo, RegisterViewModel model, string randomToken)
        {
            if (!string.IsNullOrEmpty(model.RegisterEmail) && !string.IsNullOrEmpty(model.RegisterPassword))
            {
                return repo.SaveUserTemp(new UserTemp
                {
                    Date = DateTime.Now,
                    Email = model.RegisterEmail,
                    Password = model.RegisterPassword,
                    Token = randomToken
                });
            }
            else
            {
                return null;
            }
        }

        public string GetInviteUserEmailContent(RegisterViewModel model, string action)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("<h2 style=\"border-bottom: 2px solid #bbcc28; font-weight: normal; font-family: verdana, arial, sans-serif; color: #515151;\">" + Resources.Hello + "</h2>");
            builder.AppendFormat("<p>" + Resources.ThanksForRegistering + "</p>", repoConfig.Get(ConfigurationKeyStatic.APPLICATION_NAME));
            builder.AppendFormat("<p>" + Resources.ClickToFinish + " <a href=\"{0}\">" + Resources.Here + ".</a></p>", action);
            builder.Append("<p style=\"background: #f6f5f3; border-top:2px solid #d0d0d0;border-bottom:2px solid #d0d0d0; padding: 10px\">" + Resources.IfReceivedMistake + ".</p>");
            builder.Append("<p>" + Resources.Regards + "<br>");
            builder.AppendFormat(Resources.Team + "</p>", repoConfig.Get(ConfigurationKeyStatic.APPLICATION_NAME));
            return builder.ToString();
        }

        public string GetInviteUserEmailSubject()
        {
            StringBuilder builder = new StringBuilder();
            return builder.AppendFormat(Resources.ConfirmationRegistration, repoConfig.Get(ConfigurationKeyStatic.APPLICATION_NAME)).ToString();
        }

        public string GetRemindUserEmailSubject()
        {
            StringBuilder builder = new StringBuilder();
            return builder.AppendFormat(Resources.ResettingPassword, repoConfig.Get(ConfigurationKeyStatic.APPLICATION_NAME)).ToString();
        }

        public string GetRemindUserEmailContent(RemindViewModel model, string action, User user)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("<h2 style=\"border-bottom: 2px solid #bbcc28; font-weight: normal; font-family: verdana, arial, sans-serif; color: #515151;\">" + Resources.Hello + "</h2>", user.Nick);
            builder.AppendFormat("<p>" + Resources.ToResetPassword + " <a href=\"{0}\">" + Resources.Here + ".</a></p>", action);
            builder.Append("<p style=\"background: #f6f5f3; border-top:2px solid #d0d0d0;border-bottom:2px solid #d0d0d0; padding: 10px\">" + Resources.IfReceivedMistake + "</p>");
            builder.Append("<p>" + Resources.Regards + "<br>");
            builder.AppendFormat(Resources.Team + "</p>", repoConfig.Get(ConfigurationKeyStatic.APPLICATION_NAME)).ToString();
            return builder.ToString();
        }

        public bool SendInviteUserEmail(RegisterViewModel model, string action)
        {
            sender.IsBodyHtml(true);
            sender.AddTo(model.RegisterEmail);
            sender.SetSubject(this.GetInviteUserEmailSubject());
            sender.SetContent(this.GetInviteUserEmailContent(model, action));
            if (sender.SendEmail())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SendRemindUserEmail(RemindViewModel model, string action, User user)
        {
            sender.IsBodyHtml(true);
            sender.AddTo(model.Email);
            sender.SetSubject(this.GetRemindUserEmailSubject());
            sender.SetContent(this.GetRemindUserEmailContent(model, action, user));
            if (sender.SendEmail())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsSetUser(User userByEmail, User userByNick, RegisterViewModel model)
        {
            return userByEmail != null || userByNick != null;
        }

        public string GetUniqueToken(string token, IUserRepository repo)
        {
            UserTemp userTemp = repo.UserTempByToken(token);
            if (userTemp != null)
            {
                return this.GetUniqueToken(textBuilder.GetRandomString(), repo);
            }
            else
            {
                return token;
            }
        }

        public UserToken GetNewUserToken(IUserRepository repo, User user, string randomToken)
        {
            return repo.SaveUserToken(new UserToken
            {
                ExpiryDate = DateTime.Now.AddHours(3),
                UserID = user.UserID,
                Token = randomToken
            });
        }

        public bool IsTokenValid(DateTime date)
        {
            return DateTime.Now < date;
        }

    }
}