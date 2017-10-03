using LuzzedroCMS.Domain.Abstract;
using LuzzedroCMS.Infrastructure.Abstract;
using LuzzedroCMS.WebUI.Infrastructure.Static;
using System.Web.Mvc;

namespace LuzzedroCMS.Controllers
{
    public class HomeController : Controller
    {
        private IConfigurationKeyRepository repoConfig;

        public HomeController(IConfigurationKeyRepository configRepo)
        {
            repoConfig = configRepo;
        }

        [HttpGet]
        public ViewResult Index()
        {
            ViewBag.Title = repoConfig.Get(ConfigurationKeyStatic.MAIN_TITLE);
            ViewBag.Description = repoConfig.Get(ConfigurationKeyStatic.MAIN_DESCRIPTION);
            return View();
        }
    }
}