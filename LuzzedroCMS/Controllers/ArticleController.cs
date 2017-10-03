using LuzzedroCMS.Domain.Abstract;
using LuzzedroCMS.Domain.Entities;
using LuzzedroCMS.Infrastructure.Abstract;
using LuzzedroCMS.Models;
using LuzzedroCMS.WebUI.Infrastructure.Enums;
using LuzzedroCMS.WebUI.Infrastructure.Helpers;
using LuzzedroCMS.WebUI.Infrastructure.Static;
using LuzzedroCMS.WebUI.Properties;
using LuzzedroCMS.WebUI.ViewModels;
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
        public int PageSize = 2;

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
            IList<ArticleExtended> latestArticles = repoArticle.ArticlesExtended(take: 11);
            IList<ArticleExtended> articlesByCategorySection1 = repoCategory.Categories(page: 1, take: 1).FirstOrDefault() != null ? repoArticle.ArticlesExtended(categoryID: repoCategory.Categories(page: 1, take: 1).FirstOrDefault().CategoryID, take: 4) : null;
            IList<ArticleExtended> articlesByCategorySection2 = repoCategory.Categories(page: 2, take: 1).FirstOrDefault() != null ? repoArticle.ArticlesExtended(categoryID: repoCategory.Categories(page: 2, take: 1).FirstOrDefault().CategoryID, take: 4) : null;
            IList<ArticleExtended> articlesByCategorySection3 = repoCategory.Categories(page: 3, take: 1).FirstOrDefault() != null ? repoArticle.ArticlesExtended(categoryID: repoCategory.Categories(page: 3, take: 1).FirstOrDefault().CategoryID, take: 4) : null;
            IList<ArticleExtended> articlesByCategorySection4 = repoCategory.Categories(page: 4, take: 1).FirstOrDefault() != null ? repoArticle.ArticlesExtended(categoryID: repoCategory.Categories(page: 4, take: 1).FirstOrDefault().CategoryID, take: 4) : null;
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
            ArticlesExtendedViewModel articlesExtendedViewModel = new ArticlesExtendedViewModel
            {
                ArticlesExtended = repoArticle.ArticlesExtended().OrderBy(x => Guid.NewGuid()).Take(4).ToList(),
                ContentExternalUrl = repoConfig.Get(ConfigurationKeyStatic.CONTENT_EXTERNAL_URL)
            };
            return View(articlesExtendedViewModel);
        }


        [HttpGet]
        public ActionResult Article(string category, string url)
        {
            ViewBag.HideTitle = true;
            ArticleExtended articleExtended = repoArticle.ArticleExtended(url: url);
            if (articleExtended != null)
            {
                User user = repoUser.User(email: repoSession.UserEmail);
                ArticlePageViewModel articlePageViewModel = new ArticlePageViewModel()
                {
                    Article = articleExtended.Article,
                    Category = articleExtended.Category,
                    User = articleExtended.User,
                    CommentsExtended = articleExtended.CommentsExtended,
                    ContentExternalUrl = repoConfig.Get(ConfigurationKeyStatic.CONTENT_EXTERNAL_URL),
                    HasUserNick = !string.IsNullOrEmpty(repoSession.UserNick),
                    IsLogged = repoSession.IsLogged,
                    HasBookmark = user != null ? repoArticle.BookmarkUserArticleAssociate(
                        articleID: articleExtended.Article.ArticleID,
                        userID: user.UserID) != null : false,
                    Tags = articleExtended.Tags
                };
                ViewBag.Title = String.Format("{0} - {1}", articleExtended.Article.Title, Resources.Article);
                if (articleExtended != null)
                {
                    ViewBag.Description = articleExtended.Article.Lead;
                    ViewBag.Keywords = String.Format("{0} - {1}", articleExtended.Article.ImageDesc, Resources.Article);
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
            IList<ArticleExtended> articlesExtended = repoArticle.ArticlesExtended(tagName: tag, page: page, take: PageSize).ToList();
            if (articlesExtended.Any())
            {
                ArticlesTagViewModel articlesTagViewModel = new ArticlesTagViewModel
                {
                    ArticlesExtended = articlesExtended,
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
                if (articlesExtended.Count() != 0)
                {
                    ViewBag.Description = String.Format("{0} - {1}", articlesExtended.First().Article.Lead, Resources.ArticleByTag);
                    ViewBag.Keywords = String.Format("{0} - {1}", articlesExtended.First().Article.ImageDesc, Resources.ArticleByTag);
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
            Category Category = repoCategory.Category(categoryName: category);
            if (Category != null)
            {
                int categoryID = Category.CategoryID;
                IList<ArticleExtended> articlesExtended = repoArticle.ArticlesExtended(categoryID: categoryID, page: page, take: PageSize).ToList();
                if (articlesExtended.Any())
                {
                    ViewBag.Title = String.Format("{0} - {1}", category, Resources.ArticleByCategory);
                    if (articlesExtended.Count() != 0)
                    {
                        ViewBag.Description = String.Format("{0} - {1}", articlesExtended.First().Article.Lead, Resources.ArticleByCategory);
                        ViewBag.Keywords = String.Format("{0} - {1}", articlesExtended.First().Article.ImageDesc, Resources.ArticleByCategory);
                    }
                    return View(new ArticlesCategoryViewModel
                    {
                        ArticlesExtended = articlesExtended,
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
                repoArticle.SaveBookmark(article.ArticleID, user.UserID);
                return Resources.ProperlyAddedBookmark;
            }
        }
    }
}