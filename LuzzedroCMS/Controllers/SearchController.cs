using LuzzedroCMS.Domain.Abstract;
using LuzzedroCMS.Domain.Entities;
using LuzzedroCMS.Infrastructure.Attributes;
using LuzzedroCMS.Models;
using LuzzedroCMS.WebUI.Infrastructure.Helpers;
using LuzzedroCMS.WebUI.Infrastructure.Static;
using LuzzedroCMS.WebUI.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LuzzedroCMS.Controllers
{
    public class SearchController : Controller
    {
        private IArticleRepository repoArticle;
        private ICategoryRepository repoCategory;
        private IConfigurationKeyRepository repoConfig;
        public int PageSize = 2;

        public SearchController(
            IArticleRepository articleRepo,
            ICategoryRepository categoryRepo,
            IConfigurationKeyRepository configRepo)
        {
            repoArticle = articleRepo;
            repoCategory = categoryRepo;
            repoConfig = configRepo;
        }

        [HttpGet]
        [ChildActionOnly]
        public ViewResult Index()
        {
            return View();
        }

        [HttpGet]
        [RestoreModelStateFromTempData]
        public ActionResult Result(SearchViewModel searchViewModel, int page = 1)
        {
            if (ModelState.IsValid)
            {
                IList<ArticleExtended> articlesExtended = repoArticle.ArticlesExtended(key: searchViewModel.Key, page: page, take: PageSize).OrderBy(x => x.Article.DateAdd).ToList();
                ArticlesSearchedViewModel articleSearchViewModel = new ArticlesSearchedViewModel
                {
                    SearchKey = searchViewModel.Key,
                    ArticlesExtended = articlesExtended,
                    ContentExternalUrl = repoConfig.Get(ConfigurationKeyStatic.CONTENT_EXTERNAL_URL),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = PageSize,
                        TotalItems = repoArticle.Articles(key: searchViewModel.Key).Count()
                    }
                };
                ViewBag.Title = String.Format("{0} - {1}", searchViewModel.Key, Resources.SearchResult);
                if (articlesExtended.Count() != 0)
                {
                    ViewBag.Description = String.Format("{0} - {1}", articlesExtended.First().Article.Lead, Resources.SearchResult);
                    ViewBag.Keywords = String.Format("{0} - {1}", articlesExtended.First().Article.ImageDesc, Resources.ArticleByCategory);
                }
                return View(articleSearchViewModel);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [SetTempDataModelState]
        public ActionResult Result(SearchViewModel searchViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Title = String.Format("{0} - {1}", searchViewModel.Key, Resources.SearchResult);
                ViewBag.Description = String.Format("{0} - {1}", searchViewModel.Key, Resources.SearchResult);
                return Redirect("Search?Key=" + searchViewModel.Key);
            }
            else
            {
                return Redirect(returnUrl ?? Url.Action("Index", "Home"));
            }
        }
    }
}