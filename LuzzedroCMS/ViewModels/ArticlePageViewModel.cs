using LuzzedroCMS.Domain.Entities;
using System.Collections.Generic;
using LuzzedroCMS.WebUI.ViewModels;
using System.Linq;

namespace LuzzedroCMS.Models
{
    public class ArticlePageViewModel : ArticleExtended
    {
        public string ContentExternalUrl { get; set; }
        public bool HasUserNick { get; set; }
        public bool IsLogged { get; set; }
        public bool HasBookmark { get; set; }
    }
}