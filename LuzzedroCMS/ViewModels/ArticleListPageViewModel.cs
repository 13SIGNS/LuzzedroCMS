using LuzzedroCMS.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace LuzzedroCMS.Models
{
    public class ArticleListPageViewModel
    {
        public IList<Article> LatestArticles { get; set; }
        public IList<Article> ArticlesByCategorySection1 { get; set; }
        public IList<Article> ArticlesByCategorySection2 { get; set; }
        public IList<Article> ArticlesByCategorySection3 { get; set; }
        public IList<Article> ArticlesByCategorySection4 { get; set; }
        public string ContentExternalUrl { get; set; }
    }
}