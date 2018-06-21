using LuzzedroCMS.Domain.Entities;
using LuzzedroCMS.WebUI.Infrastructure.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace LuzzedroCMS.Models
{
    public class ArticlesSearchedViewModel : ArticlesViewModel
    {
        public string SearchKey { get; set; }
    }
}