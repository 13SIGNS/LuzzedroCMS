using LuzzedroCMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuzzedroCMS.Models
{
    public class ArticlesSearchViewModel
    {
        public IQueryable<Article> Articles { get; set; }
        public IList<Category> Categories { get; set; }
        public string SearchKey { get; set; }
    }
}