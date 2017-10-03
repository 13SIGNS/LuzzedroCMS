using LuzzedroCMS.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace LuzzedroCMS.Models
{
    public class ArticleListPageViewModel
    {
        public IList<ArticleExtended> LatestArticles { get; set; }
        public IList<ArticleExtended> ArticlesByCategorySection1 { get; set; }
        public IList<ArticleExtended> ArticlesByCategorySection2 { get; set; }
        public IList<ArticleExtended> ArticlesByCategorySection3 { get; set; }
        public IList<ArticleExtended> ArticlesByCategorySection4 { get; set; }
        public string ContentExternalUrl { get; set; }
    }
}