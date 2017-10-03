using LuzzedroCMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuzzedroCMS.Models
{
    public class CommentExtendedWithArticleViewModel
    {
        public CommentExtended CommentExtended { get; set; }
        public ArticleExtended ArticleExtended { get; set; }
    }
}