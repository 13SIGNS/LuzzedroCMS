using LuzzedroCMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuzzedroCMS.Models
{
    public class ArticleListViewModel
    {
        public IQueryable<Article> LatestArticles { get; set; }
        public IList<string> LatestArticlesCategories { get; set; }
        public IQueryable<Article> ArticlesByCategorySection1 { get; set; }
        public IList<string> ArticlesByCategorySection1Categories { get; set; }
        public IQueryable<Article> ArticlesByCategorySection2 { get; set; }
        public IList<string> ArticlesByCategorySection2Categories { get; set; }
        public IQueryable<Article> ArticlesByCategorySection3 { get; set; }
        public IList<string> ArticlesByCategorySection3Categories { get; set; }
        public IQueryable<Article> ArticlesByCategorySection4 { get; set; }
        public IList<string> ArticlesByCategorySection4Categories { get; set; }
    }
}