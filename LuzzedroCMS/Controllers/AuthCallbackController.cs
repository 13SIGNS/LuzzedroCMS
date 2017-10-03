using LuzzedroCMS.Domain.Abstract;
using LuzzedroCMS.WebUI.Infrastructure.Authorization;
using LuzzedroCMS.WebUI.Infrastructure.Static;
using System.Web.Mvc;

namespace LuzzedroCMS.WebUI.Controllers
{
    public class AuthCallbackController : Google.Apis.Auth.OAuth2.Mvc.Controllers.AuthCallbackController
    {
        private IConfigurationKeyRepository repoConfig;

        public AuthCallbackController(
            IConfigurationKeyRepository configRepo)
        {
            repoConfig = configRepo;
        }

        protected override Google.Apis.Auth.OAuth2.Mvc.FlowMetadata FlowData
        {
            get
            {
                return new AppFlowMetadata(repoConfig.Get(ConfigurationKeyStatic.GOOGLE_APP_ID), repoConfig.Get(ConfigurationKeyStatic.GOOGLE_APP_SECRET));
            }
        }
    }
}