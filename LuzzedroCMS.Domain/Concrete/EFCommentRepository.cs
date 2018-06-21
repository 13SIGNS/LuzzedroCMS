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

        public Comment Comment(
            bool enabled = true,
            int commentID = 0,
            Comment comment = null)
        {
            Comment commentSelected = comment != null ? comment : Comment(enabled, commentID);
            return commentSelected;
        }

        public IList<Comment> commentsSelected(
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

        public IList<Comment> Comments(
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

            return commentsSelected;
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
                    User = comment.User,
                    ParentCommentID = comment.ParentCommentID,
                    Article = comment.Article,
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
                    dbEntry.User = comment.User;
                    dbEntry.Article = comment.Article;
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