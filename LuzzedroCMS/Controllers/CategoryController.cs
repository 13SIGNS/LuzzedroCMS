using LuzzedroCMS.Domain.Abstract;
using LuzzedroCMS.Domain.Entities;
using LuzzedroCMS.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace LuzzedroCMS.Controllers
{
    public class CategoryController : Controller
    {
        private ICategoryRepository repoCategory;

        public CategoryController(ICategoryRepository categoryRepo)
        {
            repoCategory = categoryRepo;
        }

        [HttpGet]
        [ChildActionOnly]
        public ActionResult CategoryList()
        {
            IList<Category> categories = repoCategory.Categories();
            IList<CategoryViewModel> categoriesViewModel = new List<CategoryViewModel>();
            foreach (Category category in categories)
            {
                categoriesViewModel.Add(new CategoryViewModel
                {
                    CategoryID = category.CategoryID,
                    Name = category.Name,
                    Order = category.Order,
                    Status = category.Status
                });
            }
            return View(categoriesViewModel);
        }

        [HttpGet]
        [ChildActionOnly]
        public ActionResult CategoryListSimplified()
        {
            return View(repoCategory.Categories());
        }
    }
}