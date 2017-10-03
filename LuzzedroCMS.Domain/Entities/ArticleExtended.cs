using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuzzedroCMS.Domain.Entities
{
    public class ArticleExtended
    {
        public Article Article { get; set; }
        public Category Category { get; set; }
        public User User { get; set; }
        public IList<Tag> Tags { get; set; }
        public IList<CommentExtended> CommentsExtended { get; set; }
    }
}