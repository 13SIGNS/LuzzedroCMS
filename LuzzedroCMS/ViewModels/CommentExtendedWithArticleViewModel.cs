using LuzzedroCMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuzzedroCMS.Models
{
    public class CommentWithArticleViewModel
    {
        public Comment Comment { get; set; }
        public Article Article { get; set; }
    }
}