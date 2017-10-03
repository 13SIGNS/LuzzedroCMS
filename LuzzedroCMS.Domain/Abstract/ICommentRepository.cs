using LuzzedroCMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LuzzedroCMS.Domain.Abstract
{
    public interface ICommentRepository
    {
        Comment Comment(
            bool enabled = true,
            int commentID = 0);

        CommentExtended CommentExtended(
            bool enabled = true,
            int commentID = 0,
            Comment comment = null);

        IList<Comment> Comments(
            bool enabled = true,
            int page = 1,
            int take = 0,
            int articleID = 0,
            int userID = 0,
            Expression<Func<Comment, bool>> orderBy = null,
            Expression<Func<Comment, bool>> orderByDescending = null);

        IList<CommentExtended> CommentsExtended(
            bool enabled = true,
            int page = 1,
            int take = 0,
            int articleID = 0,
            int userID = 0,
            Expression<Func<Comment, bool>> orderBy = null,
            Expression<Func<Comment, bool>> orderByDescending = null,
            IList<Comment> comments = null);

        int Save(Comment comment);
        void Remove(int commentID);
        void RemovePermanently(int commentID);
    }
}