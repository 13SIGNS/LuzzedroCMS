using LuzzedroCMS.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LuzzedroCMS.Domain.Entities;

namespace LuzzedroCMS.Domain.Concrete
{
    public class EFCommentRepository : ICommentRepository
    {
        private EFDbContext context = new EFDbContext();

        public Comment CommentByID(int commentID)
        {
            return context.Comments.Find(commentID);
        }

        public IQueryable<Comment> CommentsEnabled
        {
            get
            {
                return context.Comments.Where(p => p.Status == 1);
            }
        }

        public IQueryable<Comment> CommentsTotal
        {
            get
            {
                return context.Comments;
            }
        }

        public IQueryable<Comment> CommentsEnabledByArticleID(int articleID)
        {
            return context.Comments.Where(p => p.ArticleID == articleID && p.Status == 1);
        }

        public IQueryable<Comment> CommentsEnabledByUserID(int userID)
        {
            return context.Comments.Where(p => p.UserID == userID && p.Status == 1).OrderByDescending(p => p.Date);
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