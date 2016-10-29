using LuzzedroCMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuzzedroCMS.Models
{
    public class ArticlesCategoriesViewModel
    {
        public IQueryable<Article> Articles { get; set; }
        public IList<string> Categories { get; set; }
    }
}