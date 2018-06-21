using LuzzedroCMS.Domain.Abstract;
using LuzzedroCMS.Domain.Entities;
using LuzzedroCMS.Infrastructure.Abstract;
using LuzzedroCMS.Models;
using LuzzedroCMS.WebUI.Infrastructure.Enums;
using LuzzedroCMS.WebUI.Infrastructure.Helpers;
using LuzzedroCMS.WebUI.Infrastructure.Static;
using LuzzedroCMS.WebUI.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LuzzedroCMS.Controllers
{
    public class ArticleController : Controller
    {
        private IArticleRepository repoArticle;
        private ICategoryRepository repoCategory;
        private ICommentRepository repoComment;
        private IUserRepository repoUser;
        private IConfigurationKeyRepository repoConfig;
        private ISessionHelper repoSession;
        public int PageSize = 10;

        public ArticleController(
            IArticleRepository articleRepo,
            ICategoryRepository categoryRepo,
            ICommentRepository commentRepo,
            IUserRepository userRepo,
            IConfigurationKeyRepository configRepo,
            ISessionHelper sessionRepo)
        {
            repoArticle = articleRepo;
            repoCategory = categoryRepo;
            repoComment = commentRepo;
            repoUser = userRepo;
            repoConfig = configRepo;
            repoSession = sessionRepo;
            repoSession.Controller = this;
        }

        [HttpGet]
        public ViewResult Index()
        {
            ViewBag.HideTitle = true;
            IList<Article> latestArticles = repoArticle.Articles(take: 11);
            IList<Article> articlesByCategorySection1 = repoCategory.Categories(page: 1, take: 1).FirstOrDefault() != null ? repoArticle.Articles(categoryID: repoCategory.Categories(page: 1, take: 1).FirstOrDefault().CategoryID, take: 4) : null;
            IList<Article> articlesByCategorySection2 = repoCategory.Categories(page: 2, take: 1).FirstOrDefault() != null ? repoArticle.Articles(categoryID: repoCategory.Categories(page: 2, take: 1).FirstOrDefault().CategoryID, take: 4) : null;
            IList<Article> articlesByCategorySection3 = repoCategory.Categories(page: 3, take: 1).FirstOrDefault() != null ? repoArticle.Articles(categoryID: repoCategory.Categories(page: 3, take: 1).FirstOrDefault().CategoryID, take: 4) : null;
            IList<Article> articlesByCategorySection4 = repoCategory.Categories(page: 4, take: 1).FirstOrDefault() != null ? repoArticle.Articles(categoryID: repoCategory.Categories(page: 4, take: 1).FirstOrDefault().CategoryID, take: 4) : null;
            ArticleListPageViewModel articleListViewModel = new ArticleListPageViewModel
            {
                LatestArticles = latestArticles,
                ArticlesByCategorySection1 = articlesByCategorySection1,
                ArticlesByCategorySection2 = articlesByCategorySection2,
                ArticlesByCategorySection3 = articlesByCategorySection3,
                ArticlesByCategorySection4 = articlesByCategorySection4,
                ContentExternalUrl = repoConfig.Get(ConfigurationKeyStatic.CONTENT_EXTERNAL_URL)
            };
            ViewBag.Title = repoConfig.Get(ConfigurationKeyStatic.MAIN_TITLE);
            ViewBag.Description = repoConfig.Get(ConfigurationKeyStatic.MAIN_DESCRIPTION);
            return View(articleListViewModel);
        }

        [HttpGet]
        [ChildActionOnly]
        public ActionResult ArticlesMore()
        {
            ArticlesViewModel articlesViewModel = new ArticlesViewModel
            {
                Articles = repoArticle.Articles().OrderBy(x => Guid.NewGuid()).Take(4).ToList(),
                ContentExternalUrl = repoConfig.Get(ConfigurationKeyStatic.CONTENT_EXTERNAL_URL)
            };
            return View(articlesViewModel);
        }


        [HttpGet]
        public ActionResult Article(string category, string url)
        {
            ViewBag.HideTitle = true;
            Article article = repoArticle.Article(url: url);
            if (article != null)
            {
                User user = repoUser.User(email: repoSession.UserEmail);
                ArticlePageViewModel articlePageViewModel = new ArticlePageViewModel()
                {
                    Category = article.Category,
                    Comments = article.Comments,
                    ContentExternalUrl = repoConfig.Get(ConfigurationKeyStatic.CONTENT_EXTERNAL_URL),
                    HasUserNick = !string.IsNullOrEmpty(repoSession.UserNick),
                    IsLogged = repoSession.IsLogged,
                    Tags = article.Tags
                };
                ViewBag.Title = String.Format("{0} - {1}", article.Title, Resources.Article);
                if (article != null)
                {
                    ViewBag.Description = article.Lead;
                    ViewBag.Keywords = String.Format("{0} - {1}", article.ImageDesc, Resources.Article);
                }
                return View(articlePageViewModel);
            }
            else
            {
                this.SetMessage(InfoMessageType.Danger, Resources.ArticleNotFound);
                return Redirect(Url.Action("Index", "Article"));
            }
        }

        [HttpGet]
        public ActionResult ArticlesByTag(string tag, int page = 1)
        {
            ViewBag.HideTitle = true;
            IList<Article> articles = repoArticle.Articles(tagName: tag, page: page, take: PageSize).ToList();
            if (articles.Any())
            {
                ArticlesTagViewModel articlesTagViewModel = new ArticlesTagViewModel
                {
                    Articles = articles,
                    TagName = tag,
                    ContentExternalUrl = repoConfig.Get(ConfigurationKeyStatic.CONTENT_EXTERNAL_URL),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = PageSize,
                        TotalItems = repoArticle.Articles(tagName: tag).Count()
                    }
                };
                ViewBag.Title = String.Format("{0} - {1}", tag, Resources.ArticleByTag);
                if (articles.Count() != 0)
                {
                    ViewBag.Description = String.Format("{0} - {1}", articles.First().Lead, Resources.ArticleByTag);
                    ViewBag.Keywords = String.Format("{0} - {1}", articles.First().ImageDesc, Resources.ArticleByTag);
                }
                return View(articlesTagViewModel);
            }
            else
            {
                this.SetMessage(InfoMessageType.Danger, Resources.ArticleNotFoundByTag);
                return Redirect(Url.Action("Index", "Article"));
            }
        }

        [HttpGet]
        public ActionResult ArticlesByCategory(string category, int page = 1)
        {
            ViewBag.HideTitle = true;
            Category Category = repoCategory.Category(categoryName: category.Replace("-", " "));
            if (Category != null)
            {
                int categoryID = Category.CategoryID;
                IList<Article> articles = repoArticle.Articles(categoryID: categoryID, page: page, take: PageSize).ToList();
                if (articles.Any())
                {
                    ViewBag.Title = String.Format("{0} - {1}", category, Resources.ArticleByCategory);
                    if (articles.Count() != 0)
                    {
                        ViewBag.Description = String.Format("{0} - {1}", articles.First().Lead, Resources.ArticleByCategory);
                        ViewBag.Keywords = String.Format("{0} - {1}", articles.First().ImageDesc, Resources.ArticleByCategory);
                    }
                    return View(new ArticlesCategoryViewModel
                    {
                        Articles = articles,
                        CategoryName = Category.Name,
                        ContentExternalUrl = repoConfig.Get(ConfigurationKeyStatic.CONTENT_EXTERNAL_URL),
                        PagingInfo = new PagingInfo
                        {
                            CurrentPage = page,
                            ItemsPerPage = PageSize,
                            TotalItems = repoArticle.Articles(categoryID: categoryID).Count()
                        }
                    });
                }
            }

            this.SetMessage(InfoMessageType.Danger, Resources.ArticleNotFoundByCategory);
            return Redirect(Url.Action("Index", "Article"));
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public string AddBookmark(string url)
        {
            Article article = repoArticle.Article(url: url);
            User user = repoUser.User(email: repoSession.UserEmail);
            if (user == null || article == null)
            {
                return Resources.MustBeLogged;
            }
            else
            {
                repoArticle.SaveBookmark(articleID, user.UserID);
                return Resources.ProperlyAddedBookmark;
            }
        }
    }
}