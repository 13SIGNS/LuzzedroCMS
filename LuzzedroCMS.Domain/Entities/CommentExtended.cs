using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuzzedroCMS.Domain.Entities
{
    public class CommentExtended
    {
        public Comment Comment { get; set; }
        public User User { get; set; }
    }
}