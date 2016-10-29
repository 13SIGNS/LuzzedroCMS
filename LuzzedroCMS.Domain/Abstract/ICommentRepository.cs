using LuzzedroCMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuzzedroCMS.Domain.Abstract
{
    public interface ICommentRepository
    {
        Comment CommentByID(int commentID);
        IQueryable<Comment> CommentsEnabled { get; }
        IQueryable<Comment> CommentsTotal { get; }
        IQueryable<Comment> CommentsEnabledByArticleID(int articleID);
        IQueryable<Comment> CommentsEnabledByUserID(int userID);
        int Save(Comment comment);
        void Remove(int commentID);
        void RemovePermanently(int commentID);
    }
}