using LuzzedroCMS.Domain.Entities;
using LuzzedroCMS.WebUI.Infrastructure.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace LuzzedroCMS.Models
{
    public class ArticlesExtendedViewModel
    {
        public IList<ArticleExtended> ArticlesExtended { get; set; }
        public string ContentExternalUrl { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}