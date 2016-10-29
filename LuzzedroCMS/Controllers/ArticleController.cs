using LuzzedroCMS.Domain.Abstract;
using LuzzedroCMS.Domain.Entities;
using LuzzedroCMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LuzzedroCMS.WebUI.Properties;

namespace LuzzedroCMS.Controllers
{
    public class ArticleController : Controller
    {
        private IArticleRepository repoArticle;
        private ICategoryRepository repoCategory;
        private ICommentRepository repoComment;
        private IUserRepository repoUser;

        public ArticleController(IArticleRepository articleRepository, ICategoryRepository categoryRepository, ICommentRepository commentRepository, IUserRepository userRepository)
        {
            repoArticle = articleRepository;
            repoCategory = categoryRepository;
            repoComment = commentRepository;
            repoUser = userRepository;
        }

        [HttpGet]
        public ViewResult Index()
        {
            ViewBag.Title = Resources.Home;
            ViewBag.HideTitle = true;
            IQueryable<Article> latestArticles = repoArticle.ArticlesEnabledActual.OrderByDescending(x => x.DatePub).Take(11);
            IList<string> latestArticlesCategories = new List<string>();
            foreach (var latestArticle in latestArticles)
            {
                latestArticlesCategories.Add(repoCategory.CategoryByID(latestArticle.CategoryID).Name);
            }

            IQueryable<Article> articlesByCategorySection1 = repoArticle.ArticlesEnabledActualByCategoryID(repoCategory.CategoriesEnabled.Take(1).Select(x => x.CategoryID).FirstOrDefault()).OrderByDescending(x => x.DatePub).Take(4);
            IList<string> articlesByCategorySection1Categories = new List<string>();
            foreach (var articleByCategorySection1 in articlesByCategorySection1)
            {
                articlesByCategorySection1Categories.Add(repoCategory.CategoryByID(articleByCategorySection1.CategoryID).Name);
            }

            IQueryable<Article> articlesByCategorySection2 = repoArticle.ArticlesEnabledActualByCategoryID(repoCategory.CategoriesEnabled.Skip(1).Take(1).Select(x => x.CategoryID).FirstOrDefault()).OrderByDescending(x => x.DatePub).Take(4);
            IList<string> articlesByCategorySection2Categories = new List<string>();
            foreach (var articleByCategorySection2 in articlesByCategorySection2)
            {
                articlesByCategorySection2Categories.Add(repoCategory.CategoryByID(articleByCategorySection2.CategoryID).Name);
            }

            IQueryable<Article> articlesByCategorySection3 = repoArticle.ArticlesEnabledActualByCategoryID(repoCategory.CategoriesEnabled.Skip(2).Take(1).Select(x => x.CategoryID).FirstOrDefault()).OrderByDescending(x => x.DatePub).Take(4);
            IList<string> articlesByCategorySection3Categories = new List<string>();
            foreach (var articleByCategorySection3 in articlesByCategorySection3)
            {
                articlesByCategorySection3Categories.Add(repoCategory.CategoryByID(articleByCategorySection3.CategoryID).Name);
            }

            IQueryable<Article> articlesByCategorySection4 = repoArticle.ArticlesEnabledActualByCategoryID(repoCategory.CategoriesEnabled.Skip(3).Take(1).Select(x => x.CategoryID).FirstOrDefault()).OrderByDescending(x => x.DatePub).Take(4);
            IList<string> articlesByCategorySection4Categories = new List<string>();
            foreach (var articleByCategorySection4 in articlesByCategorySection4)
            {
                articlesByCategorySection4Categories.Add(repoCategory.CategoryByID(articleByCategorySection4.CategoryID).Name);
            }

            ArticleListViewModel articleViewModel = new ArticleListViewModel
            {
                LatestArticles = latestArticles,
                LatestArticlesCategories = latestArticlesCategories,
                ArticlesByCategorySection1 = articlesByCategorySection1,
                ArticlesByCategorySection1Categories = articlesByCategorySection1Categories,
                ArticlesByCategorySection2 = articlesByCategorySection2,
                ArticlesByCategorySection2Categories = articlesByCategorySection2Categories,
                ArticlesByCategorySection3 = articlesByCategorySection3,
                ArticlesByCategorySection3Categories = articlesByCategorySection3Categories,
                ArticlesByCategorySection4 = articlesByCategorySection4,
                ArticlesByCategorySection4Categories = articlesByCategorySection4Categories,
            };
            return View(articleViewModel);
        }

        [HttpGet]
        public ActionResult Article(string category, string url)
        {
            ViewBag.Title = Resources.Article;
            ViewBag.HideTitle = true;
            Article article = repoArticle.ArticleByUrl(url);
            IQueryable<Comment> comments = repoComment.CommentsEnabledByArticleID(article.ArticleID);
            IList<User> users = new List<User>();
            foreach (var comment in comments)
            {
                users.Add(repoUser.UserByID(comment.UserID));
            }
            ArticleViewModel articleViewModel = new ArticleViewModel
            {
                Article = article,
                Comments = comments,
                CategoryName = category,
                Users = users
            };
            return View(articleViewModel);
        }

        [HttpGet]
        public ViewResult ArticlesByTag(string tag)
        {
            ViewBag.Title = Resources.ArticleByTag;
            ViewBag.HideTitle = true;
            IQueryable<Article> articles = repoArticle.ArticlesEnabledActualByTagName(tag);
            IList<string> categories = new List<string>();
            foreach (var article in articles)
            {
                Category category = repoCategory.CategoryByID(article.CategoryID);
                categories.Add(category.Name);
            }
            ArticlesTagViewModel articlesViewModel = new ArticlesTagViewModel
            {
                Articles = articles,
                ArticleCategories = categories,
                TagName = tag
            };
            return View(articlesViewModel);
        }

        [HttpGet]
        public ActionResult ArticlesByCategory(string category)
        {
            ViewBag.Title = Resources.ArticleByCategory;
            ViewBag.HideTitle = true;
            Category Category = repoCategory.CategoryByName(category);
            if (Category != null)
            {
                int categoryID = Category.CategoryID;
                IQueryable<Article> articles = repoArticle.ArticlesEnabledActualByCategoryID(categoryID);
                return View(new ArticlesCategoryViewModel
                {
                    Articles = articles,
                    Category = category
                });
            }
            else
            {
                return Redirect(Url.Action("Index", "Article"));
            }
        }

        public string AddBookmark(string url)
        {
            Article article = repoArticle.ArticleByUrl(url);
            User user = repoUser.UserByEmail(System.Web.HttpContext.Current.User.Identity.Name);
            if (user == null)
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