using LuzzedroCMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuzzedroCMS.Models
{
    public class ArticleViewModel
    {
        public Article Article { get; set; }
        public string CategoryName { get; set; }
        public IQueryable<Comment> Comments { get; set; }
        public IList<User> Users { get; set; }
    }
}