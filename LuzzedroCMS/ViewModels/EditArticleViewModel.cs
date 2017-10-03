using LuzzedroCMS.Domain.Entities;
using LuzzedroCMS.WebUI.Properties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LuzzedroCMS.Models
{
    public class EditArticleViewModel
    {
        public ArticleExtended ArticleExtended { get; set; }

        [Display(Name = "ChooseTags", ResourceType = typeof(Resources))]
        public IList<int> SelectedTagsId { get; set; }

        [Display(Name = "ChooseTags", ResourceType = typeof(Resources))]
        public IList<Tag> Tags { get; set; }

        [Display(Name = "Categories", ResourceType = typeof(Resources))]
        public IList<Category> Categories { get; set; }

        public IList<string> ImageNames { get; set; }

        public string ContentExternalUrl { get; set; }
    }
}