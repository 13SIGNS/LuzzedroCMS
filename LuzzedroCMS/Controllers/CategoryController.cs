using LuzzedroCMS.Domain.Abstract;
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
            return View(repoCategory.Categories());
        }

        [HttpGet]
        [ChildActionOnly]
        public ActionResult CategoryListSimplified()
        {
            return View(repoCategory.Categories());
        }
    }
}