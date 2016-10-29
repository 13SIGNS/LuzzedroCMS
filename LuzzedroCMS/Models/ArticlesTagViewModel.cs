using LuzzedroCMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuzzedroCMS.Models
{
    public class ArticlesTagViewModel
    {
        public IQueryable<Article> Articles { get; set; }
        public IList<string> ArticleCategories { get; set; }
        public string TagName { get; set; }
    }
}