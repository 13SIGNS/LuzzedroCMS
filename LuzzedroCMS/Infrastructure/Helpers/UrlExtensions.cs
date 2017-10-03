using LuzzedroCMS.Domain.Abstract;
using LuzzedroCMS.Infrastructure.Abstract;
using LuzzedroCMS.WebUI.Infrastructure.Static;
using System;
using System.Web.Mvc;

namespace LuzzedroCMS.WebUI.Infrastructure.Helpers
{
    public static class UrlExtensions
    {
        public static string AbsoluteAction(this UrlHelper url, string action, string controller, object routeValues, IConfigurationKeyRepository configRepo)
        {
            Uri requestUrl = url.RequestContext.HttpContext.Request.Url;

            string absoluteAction = string.Format("{0}{1}",
                                                  configRepo.Get(ConfigurationKeyStatic.URL),
                                                  url.Action(action, controller, routeValues));

            return absoluteAction;
        }
    }
}