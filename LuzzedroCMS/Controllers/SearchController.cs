using LuzzedroCMS.Domain.Abstract;
using LuzzedroCMS.Domain.Entities;
using LuzzedroCMS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LuzzedroCMS.Controllers
{
    public class SearchController : Controller
    {
        private IArticleRepository repoArticle;
        private ICategoryRepository repoCategory;

        public SearchController(IArticleRepository articleRepo, ICategoryRepository categoryRepo)
        {
            repoArticle = articleRepo;
            repoCategory = categoryRepo;
        }

        [HttpGet]
        [ChildActionOnly]
        public ViewResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Result(string key)
        {
                IQueryable<Article> articles = repoArticle.ArticlesEnabledActualByKey(key);
                IList<Category> categories = new List<Category>();
                foreach (Article article in articles)
                {
                    categories.Add(repoCategory.CategoryByID(article.CategoryID));
                }
                ArticlesSearchViewModel articleSearchViewModel = new ArticlesSearchViewModel
                {
                    Articles = articles,
                    SearchKey = key,
                    Categories = categories
                };
                return View(articleSearchViewModel);
        }

        [HttpPost]
        public ActionResult Result(SearchViewModel searchViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                return Redirect("Search?key="+searchViewModel.Key);
            }
            else
            {
                return Redirect(returnUrl ?? Url.Action("Index", "Home"));
            }
        }
    }
}