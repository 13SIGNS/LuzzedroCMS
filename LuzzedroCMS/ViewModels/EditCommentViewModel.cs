using LuzzedroCMS.Domain.Entities;

namespace LuzzedroCMS.Models
{
    public class EditCommentViewModel
    {
        public Comment Comment { get; set; }
        public int ArticleID { get; set; }
        public string ReturnUrl { get; set; }
    }
}