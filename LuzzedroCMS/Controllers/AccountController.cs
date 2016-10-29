using LuzzedroCMS.Abstract;
using LuzzedroCMS.Domain.Abstract;
using LuzzedroCMS.Domain.Entities;
using LuzzedroCMS.Domain.Infrastructure;
using LuzzedroCMS.Infrastructure.Concrete;
using LuzzedroCMS.Models;
using System;
using System.Configuration;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using LuzzedroCMS.WebUI.Properties;

namespace LuzzedroCMS.Controllers
{
    public class AccountController : Controller
    {
        private MyMembershipProvider myMembershipProvider;
        private IUserRepository repo;
        private IEmailSender sender;

        public AccountController(IUserRepository userRepo, IEmailSender emailSender)
        {
            myMembershipProvider = new MyMembershipProvider();
            repo = userRepo;
            sender = emailSender;
        }

        public ActionResult Logout(string returnUrl)
        {
            Session.Remove("User.Name");
            Session.Remove("User.PhotoUrl");
            FormsAuthentication.SignOut();
            TempData["Info.success"] = Resources.ProperlyLoggedOut;
            return Redirect(returnUrl ?? Url.Action("Index", "Home"));
        }

        [HttpGet]
        [ChildActionOnly]
        [RestoreModelStateFromTempData]
        public ViewResult Login()
        {
            SessionSaver sessionSaver = new SessionSaver(repo);
            sessionSaver.SetSessionUserData();
            return View();
        }

        [HttpPost]
        [SetTempDataModelState]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (!SetUserLogged(model.Email, model.Password))
                {
                    ModelState.AddModelError("", Resources.IncorectoUserNamePassword);
                }
            }
            return Redirect(returnUrl ?? Url.Action("Index", "Home"));
        }



        public bool SetUserLogged(string email, string password)
        {
            if (myMembershipProvider.ValidateUser(email, password))
            {
                TempData["Info.success"] = Resources.ProperlyLogged;
                SessionSaver sessionSaver = new SessionSaver(repo);
                sessionSaver.SetSessionUserData();
                FormsAuthentication.SetAuthCookie(email, true);
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpGet]
        [ChildActionOnly]
        [RestoreModelStateFromTempData]
        public ViewResult Register()
        {
            ViewBag.Title = Resources.Registration;
            return View();
        }

        [HttpPost]
        [SetTempDataModelState]
        public ActionResult Register(RegisterViewModel model, string returnUrl)
        {
            bool result = false;
            if (ModelState.IsValid)
            {
                User userByEmail = repo.UserByEmail(model.RegisterEmail);
                User userByNick = repo.UserByNick(model.RegisterNick);
                if (userByEmail != null || userByNick != null || (model.RegisterPassword != model.RegisterPasswordConfirm))
                {
                    string info = "";
                    if (userByEmail != null)
                    {
                        info = Resources.UserEmailExists;
                    }
                    if (userByNick != null)
                    {
                        info += Resources.UserNickExists;
                    }

                    TempData["Info.danger"] = info;
                }
                else
                {

                    TextBuilder textBuilder = new TextBuilder();
                    string randomToken = GetUniqueToken(textBuilder.GetRandomString());
                    UserTemp savedUserTemp = repo.SaveUserTemp(new UserTemp
                    {
                        Date = DateTime.Now,
                        Email = model.RegisterEmail,
                        Nick = model.RegisterNick,
                        Password = model.RegisterPassword,
                        Token = randomToken
                    });
                    if (savedUserTemp != null)
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.AppendFormat(Resources.ConfirmationRegistration, ConfigurationManager.AppSettings["ApplicationName"]);
                        sender.IsBodyHtml(true);
                        sender.AddTo(model.RegisterEmail);
                        sender.SetSubject(builder.ToString());
                        builder.Clear();
                        builder.AppendFormat("<h2 style=\"border-bottom: 2px solid #bbcc28; font-weight: normal; font-family: verdana, arial, sans-serif; color: #515151;\">"+Resources.Hello+"</h2>", model.RegisterNick);
                        builder.AppendFormat("<p>"+ Resources.ThanksForRegistering + "</p>", ConfigurationManager.AppSettings["ApplicationName"]);
                        builder.AppendFormat("<p>"+ Resources.ClickToFinish + " <a href=\"{0}\">"+Resources.Here+".</a></p>", Url.Action("ConfirmEmail", "Account", new { token = savedUserTemp.Token }, Request.Url.Scheme));
                        builder.Append("<p style=\"background: #f6f5f3; border-top:2px solid #d0d0d0;border-bottom:2px solid #d0d0d0; padding: 10px\">"+Resources.IfReceivedMistake + ".</p>");
                        builder.Append("<p>"+ Resources.Regards + "<br>");
                        builder.AppendFormat(Resources.Team+"</p>", ConfigurationManager.AppSettings["ApplicationName"]);
                        sender.SetContent(builder.ToString());
                        result = sender.SendEmail();
                        if (result)
                        {
                            TempData["Info.success"] = Resources.SentLink;
                        }
                        else
                        {
                            TempData["Info.danger"] = Resources.SentLinkProblem;
                        }
                    }
                    else
                    {
                        TempData["Info.danger"] = Resources.ErrorOcurred;
                    }
                }

            }
            return Redirect(returnUrl ?? Url.Action("Index", "Home"));
        }

        private string GetUniqueToken(string token)
        {
            TextBuilder textBuilder = new TextBuilder();
            UserTemp userTemp = repo.UserTempByToken(token);
            if (userTemp != null)
            {
                return GetUniqueToken(textBuilder.GetRandomString());
            }
            else
            {
                return token;
            }
        }


        [HttpGet]
        public ActionResult ConfirmEmail(string token)
        {
            ViewBag.Title = Resources.ConfirmingEmail;
            User user = repo.TransferUserTemp(token);
            if (user != null)
            {
                TempData["Info.success"] = Resources.CorrectlySetUp;
                SetUserLogged(user.Email, user.Password);
            }
            else
            {
                TempData["Info.danger"] = Resources.RegisterError;
            }
            return Redirect(Url.Action("Index", "Home"));
        }

        [HttpGet]
        public ViewResult Remind()
        {
            ViewBag.Title = Resources.EnterEmailReset;
            return View();
        }

        [HttpPost]
        public ActionResult Remind(RemindViewModel model, string returnUrl)
        {

            bool result = false;
            if (ModelState.IsValid)
            {
                TextBuilder textBuilder = new TextBuilder();
                try
                {
                    User user = repo.UserByEmail(model.Email);
                    string randomToken = textBuilder.GetRandomString(50);
                    UserToken userToken = repo.SaveUserToken(new UserToken
                    {
                        ExpiryDate = DateTime.Now.AddHours(3),
                        UserID = user.UserID,
                        Token = randomToken
                    });
                    if (userToken != null)
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.AppendFormat(Resources.ResettingPassword, ConfigurationManager.AppSettings["ApplicationName"]);
                        sender.IsBodyHtml(true);
                        sender.AddTo(model.Email);
                        sender.SetSubject(builder.ToString());
                        builder.Clear();
                        builder.AppendFormat("<h2 style=\"border-bottom: 2px solid #bbcc28; font-weight: normal; font-family: verdana, arial, sans-serif; color: #515151;\">" + Resources.Hello + "</h2>", user.Nick);
                        builder.AppendFormat("<p>"+ Resources.ToResetPassword+" <a href=\"{0}\">"+Resources.Here+".</a></p>", Url.Action("Reset", "Account", new { token = randomToken }, Request.Url.Scheme));
                        builder.Append("<p style=\"background: #f6f5f3; border-top:2px solid #d0d0d0;border-bottom:2px solid #d0d0d0; padding: 10px\">"+Resources.IfReceivedMistake + "</p>");
                        builder.Append("<p>"+Resources.Regards+"<br>");
                        builder.AppendFormat(Resources.Team+"</p>", ConfigurationManager.AppSettings["ApplicationName"]);
                        sender.SetContent(builder.ToString());
                        result = sender.SendEmail();
                        if (result)
                        {
                            TempData["Info.success"] = Resources.SentLinkFurther;
                        }
                        else
                        {
                            TempData["Info.danger"] = Resources.SentLinkProblem;
                        }
                    }
                }
                catch
                {
                    TempData["Info.danger"] = Resources.NoUserEmail;
                }
            }
            return Redirect(returnUrl ?? Url.Action("Index", "Home"));
        }

        [HttpGet]
        public ViewResult Reset(string token)
        {
            ViewBag.Title = Resources.PasswordReset;
            return View();
        }

        [HttpPost]
        public ActionResult Reset(ResetViewModel resetViewModel, string token, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                UserToken userToken = repo.UserTokenByToken(token);
                User user = repo.UserByToken(token);
                if (user != null)
                {
                    if (DateTime.Now > userToken.ExpiryDate)
                    {
                        user.Password = resetViewModel.Password;
                        repo.Save(user);
                        repo.RemoveUserToken(token);
                        TempData["Info.success"] = Resources.PasswordChanged;
                    }
                    else
                    {
                        TempData["Info.danger"] = Resources.CodeExpired;
                    }
                }
                else
                {
                    TempData["Info.danger"] = Resources.CodeNotExists;
                }
            }
            else
            {
                TempData["Info.danger"] = Resources.PasswordsDontMatch;
                return Redirect(Url.Action("Reset", "Account", new { token = Request.QueryString["token"] }));
            }
            return Redirect(returnUrl ?? Url.Action("Index", "Home"));
        }

    }
}