using LuzzedroCMS.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using LuzzedroCMS.WebUI.Properties;

namespace LuzzedroCMS.Models
{
    public class EditArticleViewModel
    {
        public Article Article { get; set; }

        [Display(Name = "ChooseTags", ResourceType = typeof(Resources))]
        public IQueryable<Tag> Tags { get; set; }

        [Display(Name = "SelectedCategoryIDs", ResourceType = typeof(Resources))]
        public int SelectedCategoryID { get; set; }

        [Display(Name = "SelectedTagIDs", ResourceType = typeof(Resources))]
        public IQueryable<int> SelectedTagIDs { get; set; }

        [Display(Name = "Categories", ResourceType = typeof(Resources))]
        public IQueryable<Category> Categories { get; set; }
    }
}