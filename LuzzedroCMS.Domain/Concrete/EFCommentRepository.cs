using LuzzedroCMS.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using LuzzedroCMS.Domain.Entities;
using System.Linq.Expressions;

namespace LuzzedroCMS.Domain.Concrete
{
    public class EFCommentRepository : ICommentRepository
    {
        private EFDbContext context = new EFDbContext();

        public Comment Comment(
            bool enabled = true,
            int commentID = 0)
        {
            IQueryable<Comment> comments = context.Comments;

            if (enabled)
            {
                comments = comments.Where(p => p.Status == 1);
            }

            if (commentID != 0)
            {
                comments = comments.Where(p => p.CommentID == commentID);
            }

            return comments.FirstOrDefault();
        }

        public CommentExtended CommentExtended(
            bool enabled = true,
            int commentID = 0,
            Comment comment = null)
        {
            Comment commentSelected = comment != null ? comment : Comment(enabled, commentID);
            User user = context.Users.FirstOrDefault(p => p.UserID == comment.UserID);
            return new CommentExtended()
            {
                Comment = comment,
                User = user
            };
        }

        public IList<Comment> Comments(
            bool enabled = true,
            int page = 1,
            int take = 0,
            int articleID = 0,
            int userID = 0,
            Expression<Func<Comment, bool>> orderBy = null,
            Expression<Func<Comment, bool>> orderByDescending = null)
        {
            IQueryable<Comment> comments = context.Comments;

            if (enabled)
            {
                comments = comments.Where(p => p.Status == 1);
            }

            if (articleID != 0)
            {
                comments = comments.Where(p => p.ArticleID == articleID);
            }

            if (userID != 0)
            {
                comments = comments.Where(p => p.UserID == userID);
            }

            if (orderByDescending != null)
            {
                comments = comments.OrderByDescending(orderByDescending);
            }

            if (orderBy != null)
            {
                comments = comments.OrderBy(orderBy);
            }

            if (orderBy == null && orderByDescending == null)
            {
                comments = comments.OrderByDescending(p => p.Date);
            }

            if (page != 0 && take != 0)
            {
                comments = comments.Skip((page - 1) * take);
            }

            if (take != 0)
            {
                comments = comments.Take(take);
            }

            return comments.ToList();
        }

        public IList<CommentExtended> CommentsExtended(
            bool enabled = true,
            int page = 1,
            int take = 0,
            int articleID = 0,
            int userID = 0,
            Expression<Func<Comment, bool>> orderBy = null,
            Expression<Func<Comment, bool>> orderByDescending = null,
            IList<Comment> comments = null)
        {
            IList<Comment> commentsSelected = comments != null ? comments : Comments(enabled, page, take, articleID, userID, orderBy, orderByDescending);
            IList<CommentExtended> commentsExtended = new List<CommentExtended>();
            foreach (var comment in commentsSelected)
            {
                commentsExtended.Add(new CommentExtended()
                {
                    Comment = comment,
                    User = context.Users.FirstOrDefault(p => p.UserID == comment.UserID)
                });
            }
            return commentsExtended;
        }

        public void Remove(int commentID)
        {
            Comment comment = context.Comments.Find(commentID);
            if (comment != null)
            {
                comment.Status = 0;
            }
            context.SaveChanges();
        }

        public void RemovePermanently(int commentID)
        {
            Comment comment = context.Comments.Find(commentID);
            if (comment != null)
            {
                context.Comments.Remove(comment);
            }

            context.SaveChanges();
        }

        public int Save(Comment comment)
        {
            Comment dbEntry;
            if (comment.CommentID == 0)
            {
                dbEntry = context.Comments.Add(new Comment
                {
                    UserID = comment.UserID,
                    ParentCommentID = comment.ParentCommentID,
                    ArticleID = comment.ArticleID,
                    Date = DateTime.Now,
                    Content = comment.Content,
                    Status = 1
                });
            }
            else
            {
                dbEntry = context.Comments.Find(comment.CommentID);
                if (dbEntry != null)
                {
                    dbEntry.UserID = comment.UserID;
                    dbEntry.ArticleID = comment.ArticleID;
                    dbEntry.ParentCommentID = comment.ParentCommentID;
                    dbEntry.Date = DateTime.Now;
                    dbEntry.Content = comment.Content;
                    dbEntry.Status = comment.Status;
                }
            }
            context.SaveChanges();
            return dbEntry.CommentID;
        }
    }
}