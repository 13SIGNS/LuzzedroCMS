using Facebook;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
using LuzzedroCMS.Domain.Abstract;
using LuzzedroCMS.Domain.Entities;
using LuzzedroCMS.Domain.Infrastructure.Abstract;
using LuzzedroCMS.Infrastructure.Abstract;
using LuzzedroCMS.Infrastructure.Attributes;
using LuzzedroCMS.Models;
using LuzzedroCMS.WebUI.Infrastructure.Authorization;
using LuzzedroCMS.WebUI.Infrastructure.Helpers;
using LuzzedroCMS.WebUI.Properties;
using LuzzedroCMS.WebUI.Infrastructure.Static;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using LuzzedroCMS.WebUI.Infrastructure.Enums;
using LuzzedroCMS.Domain.Infrastructure.Concrete;

namespace LuzzedroCMS.Controllers
{
    public class AccountController : Controller
    {

        private IUserRepository repo;
        private IEmailSender sender;
        private IConfigurationKeyRepository repoConfig;
        private IAccount account;
        private ITextBuilder textBuilder;
        private ISessionHelper repoSession;
        private const int SOURCE_FACEBOOK = 1;
        private const int SOURCE_GOOGLE = 2;

        public AccountController(
            IUserRepository userRepo,
            IEmailSender emailSender,
            IConfigurationKeyRepository configRepo,
            IAccount accnt,
            ITextBuilder txtBuilder,
            ISessionHelper sessionRepo)
        {
            repo = userRepo;
            sender = emailSender;
            repoConfig = configRepo;
            account = accnt;
            textBuilder = txtBuilder;
            repoSession = sessionRepo;
            repoSession.Controller = this;
        }

        [HttpGet]
        [ChildActionOnly]
        [Authorize(Roles = "Admin, User")]
        public ViewResult LoggedInfo()
        {
            return View(new LoginViewModel
            {
                ContentExternalUrl = repoConfig.Get(ConfigurationKeyStatic.CONTENT_EXTERNAL_URL),
                ImageName = repoSession.UserPhotoUrl,
                UserName = repoSession.UserNick
            });
        }

        [HttpGet]
        public ActionResult Logout(string returnUrl)
        {
            account.Logout(this);
            this.SetMessage(InfoMessageType.Success, Resources.ProperlyLoggedOut);
            return Redirect(returnUrl ?? Url.Action("Index", "Home"));
        }

        [HttpGet]
        public ActionResult LoginByFacebook(string returnUrl)
        {
            TempData["returnUrl"] = returnUrl;
            var fb = new FacebookClient();
            var uriBuilder = new UriBuilder(Request.Url);
            uriBuilder.Query = null;
            uriBuilder.Fragment = null;
            uriBuilder.Path = Url.Action("FacebookCallback");
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = repoConfig.Get(ConfigurationKeyStatic.FACEBOOK_APP_ID),
                client_secret = repoConfig.Get(ConfigurationKeyStatic.FACEBOOK_APP_SECRET),
                redirect_uri = uriBuilder.Uri.AbsoluteUri,
                response_type = "code",
                scope = "email"
            });
            return Redirect(loginUrl.AbsoluteUri);
        }

        [HttpGet]
        public async Task<ActionResult> LoginByGoogle(CancellationToken cancellationToken, string returnUrl)
        {
            if (Session["UserRandomId"] == null)
            {
                Session["UserRandomId"] = new TextBuilder().GetRandomString(50);
            }

            var result = await new AuthorizationCodeMvcApp(this,
                new AppFlowMetadata(
                    repoConfig.Get(ConfigurationKeyStatic.GOOGLE_APP_ID),
                    repoConfig.Get(ConfigurationKeyStatic.GOOGLE_APP_SECRET))).AuthorizeAsync(cancellationToken);

            var service = new Oauth2Service(new BaseClientService.Initializer
            {
                HttpClientInitializer = result.Credential,
                ApplicationName = "ASP.NET MVC Sample"
            });

            if (result.Credential != null)
            {
                var userInfo = await service.Userinfo.Get().ExecuteAsync();
                if (!account.RegisterAndLogByEmail(userInfo.Email, SOURCE_GOOGLE, this))
                {
                    this.SetMessage(InfoMessageType.Danger, Resources.ErrorOcurred);
                }
                Session["UserRandomId"] = null;
                return Redirect(returnUrl ?? Url.Action("Index", "Home"));
            }
            else
            {
                return new RedirectResult(result.RedirectUri);
            }
        }

        [HttpGet]
        public ActionResult FacebookCallback(string code)
        {
            string returnUrl = TempData["returnUrl"].ToString();
            var fb = new FacebookClient();
            var uriBuilder = new UriBuilder(Request.Url);
            uriBuilder.Query = null;
            uriBuilder.Fragment = null;
            uriBuilder.Path = Url.Action("FacebookCallback");
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = repoConfig.Get(ConfigurationKeyStatic.FACEBOOK_APP_ID),
                client_secret = repoConfig.Get(ConfigurationKeyStatic.FACEBOOK_APP_SECRET),
                redirect_uri = uriBuilder.Uri.AbsoluteUri,
                code = code
            });
            if (result != null)
            {
                dynamic me = fb.Get("me?fields=name,email&access_token=" + result.access_token);
                string email = me.email;

                if (!account.RegisterAndLogByEmail(me.email, SOURCE_FACEBOOK, this))
                {
                    this.SetMessage(InfoMessageType.Danger, Resources.ErrorOcurred);
                }
            }
            return Redirect(returnUrl ?? Url.Action("Index", "Home"));
        }

        [HttpGet]
        [RestoreModelStateFromTempData]
        public ViewResult Log()
        {
            ViewBag.Title = Resources.LogIn;
            return View();
        }

        [HttpGet]
        [ChildActionOnly]
        [RestoreModelStateFromTempData]
        public ViewResult Login()
        {
            return View(new LoginViewModel
            {
                IsFacebookConnected = Convert.ToBoolean(repoConfig.Get(ConfigurationKeyStatic.IS_FACEBOOK_CONNECTED)),
                IsGoogleConnected = Convert.ToBoolean(repoConfig.Get(ConfigurationKeyStatic.IS_GOOGLE_CONNECTED)),
                ContentExternalUrl = repoConfig.Get(ConfigurationKeyStatic.CONTENT_EXTERNAL_URL),
                ImageName = repoSession.UserPhotoUrl,
                UserName = repoSession.UserNick,
                IsLogged = repoSession.IsLogged
            });
        }

        [HttpPost]
        [SetTempDataModelState]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (account.SetUserLogged(model.LoginEmail, model.LoginPassword, repo, this))
                {
                    this.SetMessage(InfoMessageType.Success, Resources.ProperlyLogged);
                    return Redirect(returnUrl ?? Url.Action("Index", "Home"));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, Resources.IncorectoUserNamePassword);
                }
            }
            return Redirect(Url.Action("Log", "Account"));
        }

        [HttpGet]
        [ChildActionOnly]
        [RestoreModelStateFromTempData]
        public ViewResult Register()
        {
            return View(new RegisterViewModel()
            {
                IsLogged = repoSession.IsLogged
            });
        }

        [HttpPost]
        [SetTempDataModelState]
        public ActionResult Register(RegisterViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User userByEmail = repo.User(email: model.RegisterEmail);
                string randomToken = account.GetUniqueToken(textBuilder.GetRandomString(), repo);
                string action = Url.AbsoluteAction("ConfirmEmail", "Account", new { token = randomToken }, repoConfig);
                if (account.SetUserTemp(repo, model, randomToken) != null &&
                    account.SendInviteUserEmail(model, action))
                {
                    this.SetMessage(InfoMessageType.Success, Resources.SentLink);
                }
                else
                {
                    this.SetMessage(InfoMessageType.Danger, Resources.SentLinkProblem);
                }
            }
            return Redirect(returnUrl ?? Url.Action("Index", "Home"));
        }

        [HttpGet]
        public ActionResult ConfirmEmail(string token)
        {
            ViewBag.Title = Resources.ConfirmingEmail;
            User user = repo.TransferUserTemp(token);
            if (user != null)
            {
                this.SetMessage(InfoMessageType.Success, Resources.CorrectlySetUp);
                if (!account.SetUserLogged(user.Email, user.Password, repo, this))
                {
                    this.SetMessage(InfoMessageType.Danger, Resources.ErrorOcurred);
                }
            }
            else
            {
                this.SetMessage(InfoMessageType.Danger, Resources.RegisterError);
            }
            return Redirect(Url.Action("Index", "Home"));
        }

        [HttpGet]
        [RestoreModelStateFromTempData]
        public ViewResult Remind()
        {
            ViewBag.Title = Resources.EnterEmailReset;
            return View();
        }

        [HttpPost]
        [SetTempDataModelState]
        public ActionResult Remind(RemindViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User user = repo.User(email: model.Email);
                string randomToken = textBuilder.GetRandomString(50);
                UserToken userToken = account.GetNewUserToken(repo, user, randomToken);
                string action = Url.AbsoluteAction("Reset", "Account", new { token = randomToken }, repoConfig);
                if (userToken != null)
                {
                    if (account.SendRemindUserEmail(model, action, user))
                    {
                        this.SetMessage(InfoMessageType.Success, Resources.SentLinkFurther);
                    }
                    else
                    {
                        this.SetMessage(InfoMessageType.Danger, Resources.SentLinkProblem);
                    }
                }
                else
                {
                    this.SetMessage(InfoMessageType.Danger, Resources.NoUserEmail);
                }
            }
            return Redirect(returnUrl ?? Url.Action("Index", "Home"));
        }

        [HttpGet]
        [RestoreModelStateFromTempData]
        public ActionResult Reset(string token)
        {
            ViewBag.Title = Resources.InventPassword;
            UserToken userToken = repo.UserTokenByToken(token);
            User user = repo.User(token: token);
            if (user == null)
            {
                this.SetMessage(InfoMessageType.Danger, Resources.CodeNotExists);
                return Redirect(Url.Action("Index", "Home"));
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [SetTempDataModelState]
        public ActionResult Reset(ResetPasswordViewModel resetPasswordViewModel, string token, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                UserToken userToken = repo.UserTokenByToken(token);
                User user = repo.User(token: token);
                if (user != null)
                {
                    if (account.IsTokenValid(userToken.ExpiryDate))
                    {
                        user.Password = resetPasswordViewModel.Password;
                        repo.Save(user);
                        repo.RemoveUserToken(token);
                        this.SetMessage(InfoMessageType.Success, Resources.PasswordChanged);
                    }
                    else
                    {
                        this.SetMessage(InfoMessageType.Danger, Resources.CodeExpired);
                    }
                }
                else
                {
                    this.SetMessage(InfoMessageType.Danger, Resources.CodeNotExists);
                }
            }
            else
            {
                this.SetMessage(InfoMessageType.Danger, Resources.PasswordsDontMatch);
                return Redirect(Url.Action("Reset", "Account", new { token = Request.QueryString["token"] }));
            }
            return Redirect(returnUrl ?? Url.Action("Index", "Home"));
        }

    }
}