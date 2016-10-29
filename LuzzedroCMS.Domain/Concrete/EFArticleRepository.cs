using LuzzedroCMS.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LuzzedroCMS.Domain.Entities;
using LuzzedroCMS.Domain.Infrastructure;
using System.IO;

namespace LuzzedroCMS.Domain.Concrete
{
    public class EFArticleRepository : IArticleRepository
    {
        private EFDbContext context = new EFDbContext();
        private TextBuilder textBuilder = new TextBuilder();

        public string GetUniqueArticleTitle(string title)
        {
            int dbEntry = context.Articles.Where(p => p.Url == title).Select(p => p.ArticleID).FirstOrDefault();
            if (dbEntry != 0)
            {
                return GetUniqueArticleTitle(textBuilder.RandomizeText(title));
            }
            else
            {
                return title;
            }
        }

        public string GetUniqueImageTitle(string title)
        {
            int dbEntry = context.Articles.Where(p => p.ImageName == title).Select(p => p.ArticleID).FirstOrDefault();
            if (dbEntry != 0)
            {
                string imageName = Path.GetFileNameWithoutExtension("~/Content/ArticleImage/" + title);
                string ext = Path.GetExtension("~/Content/ArticleImage/" + title);
                return GetUniqueImageTitle(textBuilder.RandomizeText(imageName) + ext);
            }
            else
            {
                return title;
            }
        }

        public IQueryable<Article> ArticlesEnabled
        {
            get
            {
                return context.Articles.Where(p => p.Status == 1);
            }
        }

        public IQueryable<Article> Articles
        {
            get
            {
                return context.Articles;
            }
        }

        public IQueryable<Article> ArticlesEnabledActual
        {
            get
            {
                return context.Articles.Where(p => p.Status == 1 && p.DatePub < DateTime.Now && p.DateExp > DateTime.Now);
            }
        }

        public IQueryable<Article> ArticlesTotal
        {
            get
            {
                return context.Articles;
            }
        }

        public Article ArticleByUrl(string url)
        {
            return context.Articles.Where(p => p.Url == url).FirstOrDefault();
        }

        public IQueryable<Article> ArticlesEnabledActualByCategoryID(int categoryID)
        {
            return context.Articles.Where(p => p.CategoryID == categoryID && p.Status == 1);
        }

        public IQueryable<Article> ArticlesEnabledActualByTagName(string tagName)
        {
            Tag tag = context.Tags.Where(x => x.Name == tagName).FirstOrDefault();
            IQueryable<int> articleIDs = context.ArticleTagAssociates.Where(p => p.TagID == tag.TagID).Select(p => p.ArticleID);
            IList<Article> articles = new List<Article>();
            foreach (var articleID in articleIDs)
            {
                Article article = context.Articles.Find(articleID);
                if (article != null)
                {
                    articles.Add(context.Articles.Find(articleID));
                }
            }
            return articles.AsQueryable();
        }

        public IQueryable<Article> ArticlesEnabledActualByKey(string key)
        {
            return context.Articles.Where(p => p.Status == 1 && p.DatePub < DateTime.Now && p.DateExp > DateTime.Now && (p.Content.Contains(key) || p.Title.Contains(key)));
        }


        public IQueryable<Article> ArticlesEnabledActualByUserID(int userID)
        {
            return context.Articles.Where(p => p.UserID == userID && p.Status == 1);
        }

        public IQueryable<Article> ArticlesEnabledActualByBookmark(int userID)
        {
            IQueryable<BookmarkUserArticleAssociate> bookmarkUserArticleAssociates = context.BookmarkUserArticleAssociates.Where(p => p.UserID == userID);
            IList<Article> articles = new List<Article>();
            foreach (var bookmarkUserArticleAssociate in bookmarkUserArticleAssociates)
            {
                Article article = context.Articles.Find(bookmarkUserArticleAssociate.ArticleID);
                if (article != null)
                {
                    articles.Add(context.Articles.Find(bookmarkUserArticleAssociate.ArticleID));
                }
            }
            return articles.AsQueryable();
        }

        public Article ArticleEnabledActualByComment(int commentID)
        {
            Comment comment = context.Comments.Find(commentID);
            return context.Articles.Find(comment.ArticleID);
        }

        public void Remove(int articleID)
        {
            Article article = context.Articles.Find(articleID);
            if (article != null)
            {
                article.Status = 0;
            }

            IQueryable<BookmarkUserArticleAssociate> bookmarkUserArticleAssociates = context.BookmarkUserArticleAssociates.Where(p => p.ArticleID == articleID);
            if (bookmarkUserArticleAssociates.Any())
            {
                foreach (var bookmarkUserArticleAssociate in bookmarkUserArticleAssociates)
                {
                    bookmarkUserArticleAssociate.Status = 0;
                }
            }
            IQueryable<ArticleTagAssociate> articleTagAssociates = context.ArticleTagAssociates.Where(p => p.ArticleID == articleID);
            if (articleTagAssociates.Any())
            {
                foreach (var articleTagAssociate in articleTagAssociates)
                {
                    articleTagAssociate.Status = 0;
                }
            }
            IQueryable<Comment> comments = context.Comments.Where(p => p.ArticleID == articleID);
            if (comments.Any())
            {
                foreach (var comment in comments)
                {
                    comment.Status = 0;
                }
            }
            context.SaveChanges();
        }

        public void RemovePermanently(int articleID)
        {
            Article article = context.Articles.Find(articleID);
            if (article != null)
            {
                context.Articles.Remove(article);
            }

            IQueryable<BookmarkUserArticleAssociate> bookmarkUserArticleAssociates = context.BookmarkUserArticleAssociates.Where(p => p.ArticleID == articleID);
            if (bookmarkUserArticleAssociates.Any())
            {
                foreach (var bookmarkUserArticleAssociate in bookmarkUserArticleAssociates)
                {
                    context.BookmarkUserArticleAssociates.Remove(bookmarkUserArticleAssociate);
                }
            }
            IQueryable<ArticleTagAssociate> articleTagAssociates = context.ArticleTagAssociates.Where(p => p.ArticleID == articleID);
            if (articleTagAssociates.Any())
            {
                foreach (var articleTagAssociate in articleTagAssociates)
                {
                    context.ArticleTagAssociates.Remove(articleTagAssociate);
                }
            }

            IQueryable<Comment> comments = context.Comments.Where(p => p.ArticleID == articleID);
            if (comments.Any())
            {
                foreach (var comment in comments)
                {
                    context.Comments.Remove(comment);
                }
            }
            context.SaveChanges();
        }

        public int Save(Article article)
        {
            Article dbEntry;
            if (article.ArticleID == 0)
            {
                dbEntry = context.Articles.Add(new Article
                {
                    UserID = article.UserID,
                    CategoryID = article.CategoryID,
                    ImageName = article.ImageName,
                    ImageDesc = article.ImageDesc,
                    DateAdd = DateTime.Now,
                    DatePub = article.DatePub,
                    DateExp = article.DateExp,
                    Title = article.Title,
                    Url = GetUniqueArticleTitle(textBuilder.GetUrlTitle(article.Title)),
                    Lead = article.Lead,
                    Content = article.Content,
                    Source = article.Source,
                    Status = article.Status
                });

            }
            else
            {
                dbEntry = context.Articles.Find(article.ArticleID);
                if (dbEntry != null)
                {
                    if (dbEntry.Status == 0 && article.Status == 1)
                    {

                        IQueryable<BookmarkUserArticleAssociate> bookmarkUserArticleAssociates = context.BookmarkUserArticleAssociates.Where(p => p.ArticleID == article.ArticleID);
                        if (bookmarkUserArticleAssociates.Any())
                        {
                            foreach (var bookmarkUserArticleAssociate in bookmarkUserArticleAssociates)
                            {
                                bookmarkUserArticleAssociate.Status = 1;
                            }
                        }
                        IQueryable<ArticleTagAssociate> articleTagAssociates = context.ArticleTagAssociates.Where(p => p.ArticleID == article.ArticleID);
                        if (articleTagAssociates.Any())
                        {
                            foreach (var articleTagAssociate in articleTagAssociates)
                            {
                                articleTagAssociate.Status = 1;
                            }
                        }
                        IQueryable<Comment> comments = context.Comments.Where(p => p.ArticleID == article.ArticleID);
                        if (comments.Any())
                        {
                            foreach (var comment in comments)
                            {
                                comment.Status = 1;
                            }
                        }
                        context.SaveChanges();
                    }
                    dbEntry.UserID = article.UserID;
                    dbEntry.CategoryID = article.CategoryID;
                    dbEntry.ImageName = article.ImageName;
                    dbEntry.ImageDesc = article.ImageDesc;
                    dbEntry.DatePub = article.DatePub;
                    dbEntry.DateExp = article.DateExp;
                    dbEntry.Title = article.Title;
                    dbEntry.Url = textBuilder.GetUrlTitle(article.Title);
                    dbEntry.Lead = article.Lead;
                    dbEntry.Content = article.Content;
                    dbEntry.Source = article.Source;
                    dbEntry.Status = 1;
                }
            }
            try
            {
                context.SaveChanges();
                return dbEntry.ArticleID;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }


        public void SaveBookmark(int articleID, int userID)
        {
            IQueryable<BookmarkUserArticleAssociate> bookmarkUserArticleAssociates = context.BookmarkUserArticleAssociates.Where(p => p.ArticleID == articleID && p.UserID == userID);
            if (!bookmarkUserArticleAssociates.Any())
            {
                context.BookmarkUserArticleAssociates.Add(new BookmarkUserArticleAssociate
                {
                    UserID = userID,
                    ArticleID = articleID,
                    Status = 1,
                    Date = DateTime.Now
                });
                context.SaveChanges();
            }
        }

        public void RemoveBookmark(int articleID, int userID)
        {
            IQueryable<BookmarkUserArticleAssociate> bookmarkUserArticleAssociates = context.BookmarkUserArticleAssociates.Where(p => p.ArticleID == articleID && p.UserID == userID);
            if (bookmarkUserArticleAssociates.Any())
            {
                foreach (var bookmarkUserArticleAssociate in bookmarkUserArticleAssociates)
                {
                    context.BookmarkUserArticleAssociates.Remove(bookmarkUserArticleAssociate);
                }
            }
        }

        public void AddTagToArticle(int articleID, int tagID)
        {
            IQueryable<ArticleTagAssociate> articleTagAssociates = context.ArticleTagAssociates.Where(p => p.ArticleID == articleID && p.TagID == tagID);
            if (!articleTagAssociates.Any())
            {
                context.ArticleTagAssociates.Add(new ArticleTagAssociate
                {
                    TagID = tagID,
                    ArticleID = articleID,
                    Status = 1
                });
                context.SaveChanges();
            }
        }

        public void RemoveTagFromArticle(int articleID, int tagID)
        {
            IQueryable<ArticleTagAssociate> articleTagAssociates = context.ArticleTagAssociates.Where(p => p.ArticleID == articleID && p.TagID == tagID);
            if (articleTagAssociates.Any())
            {
                foreach (var articleTagAssociate in articleTagAssociates)
                {
                    context.ArticleTagAssociates.Remove(articleTagAssociate);
                }
            }
        }

        public void Enable(int articleID)
        {
            Article article = context.Articles.Find(articleID);
            if (article != null)
            {
                article.Status = 1;
                context.SaveChanges();
            }
        }
    }
}