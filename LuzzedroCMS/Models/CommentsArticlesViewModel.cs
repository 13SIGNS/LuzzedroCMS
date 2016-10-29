using LuzzedroCMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuzzedroCMS.Models
{
    public class CommentsArticlesViewModel
    {
        public IList<Article> Articles { get; set; }
        public IList<Category> Categories { get; set; }
        public IQueryable<Comment> Comments { get; set; }
    }
}