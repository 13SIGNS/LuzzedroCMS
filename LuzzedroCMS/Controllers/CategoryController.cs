using LuzzedroCMS.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            return PartialView(repoCategory.CategoriesEnabled);
        }
    }
}