using LuzzedroCMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuzzedroCMS.Models
{
    public class EditCommentViewModel
    {
        public Comment Comment { get; set; }
        public int ArticleID { get; set; }
        public string ReturnUrl { get; set; }
    }
}