using System;
using System.Web.Mvc;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Util.Store;
using LuzzedroCMS.Domain.Infrastructure.Concrete;

namespace LuzzedroCMS.WebUI.Infrastructure.Authorization
{
    public class AppFlowMetadata : FlowMetadata
    {
        private IAuthorizationCodeFlow flow;

        private static string AppId;
        private static string AppSecret;

        //public AppFlowMetadata() { }

        public AppFlowMetadata(string appId, string appSecret)
        {
            AppId = appId;
            AppSecret = appSecret;
            flow =
            new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = AppId,
                    ClientSecret = AppSecret
                },
                Scopes = new[] { "email" },
                DataStore = new FileDataStore("Drive.Api.Auth.Store")
            });

        }

        public override string GetUserId(Controller controller)
        {
            return controller.Session["UserRandomId"].ToString();
        }

        public override IAuthorizationCodeFlow Flow
        {
            get { return flow; }
        }
    }
}