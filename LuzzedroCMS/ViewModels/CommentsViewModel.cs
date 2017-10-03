using LuzzedroCMS.Domain.Entities;
using LuzzedroCMS.WebUI.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuzzedroCMS.Models
{
    public class CommentsViewModel
    {
        public IList<CommentExtended> CommentsExtended { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}