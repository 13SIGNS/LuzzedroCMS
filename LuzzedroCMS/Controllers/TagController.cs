using LuzzedroCMS.Domain.Abstract;
using LuzzedroCMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LuzzedroCMS.Controllers
{
    public class TagController : Controller
    {
        private IArticleRepository repoArticle;
        private ITagRepository repoTag;

        public TagController(IArticleRepository articleRepository, ITagRepository tagRepository)
        {
            repoArticle = articleRepository;
            repoTag = tagRepository;
        }

        [HttpGet]
        [ChildActionOnly]
        public ActionResult TagCloud()
        {
            IDictionary<string, int> tags = repoTag.TagsEnabledByAssociate;
            TagCloudViewModel tagCloudViewModel = new TagCloudViewModel
            {
                Tags = tags
            };
            return PartialView(tagCloudViewModel);
        }
    }
}