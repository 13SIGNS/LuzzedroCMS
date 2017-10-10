using LuzzedroCMS.Domain.Abstract;
using LuzzedroCMS.Domain.Entities;
using LuzzedroCMS.Domain.Infrastructure.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LuzzedroCMS.Domain.Concrete
{
    public class EFArticleRepository : IArticleRepository
    {
        private EFDbContext context = new EFDbContext();
        private TextBuilder textBuilder = new TextBuilder();

        public Article Article(
            bool enabled = true,
            bool actual = true,
            string url = null,
            int commentID = 0,
            int articleID = 0)
        {
            IQueryable<Article> articles = context.Articles;

            if (enabled)
            {
                articles = articles.Where(p => p.Status == 1);
            }

            if (actual)
            {
                articles = articles.Where(p => p.DatePub < DateTime.Now && p.DateExp > DateTime.Now);
            }

            if (url != null)
            {
                articles = articles.Where(p => p.Url == url);
            }

            if (commentID != 0)
            {
                Comment comment = context.Comments.Find(commentID);
                if (comment != null)
                {
                    articles = articles.Where(p => p.ArticleID == comment.ArticleID);
                }
            }

            if (articleID != 0)
            {
                articles = articles.Where(p => p.ArticleID == articleID);
            }

            return articles.FirstOrDefault();
        }

        public ArticleExtended ArticleExtended(
            bool enabled = true,
            bool actual = true,
            string url = null,
            int commentID = 0,
            int articleID = 0,
            Article article = null)
        {
            Article articleSelected = article != null ? article : Article(enabled, actual, url, commentID, articleID);
            IList<CommentExtended> commentsExtended = new List<CommentExtended>();
            IList<Tag> tags = new List<Tag>();
            Category category = new Category();

            var tagIDs = context.ArticleTagAssociates.Where(p => p.ArticleID == articleID).Select(x => x.TagID).ToList();

            if (tagIDs.Any())
            {
                tags = context.Tags.Where(p => tagIDs.Contains(p.TagID)).ToList();
            }

            if (articleSelected != null)
            {
                category = context.Categories.FirstOrDefault(p => p.CategoryID == articleSelected.CategoryID);
                foreach (var comment in context.Comments.Where(p => p.ArticleID == articleSelected.ArticleID).ToList())
                {
                    commentsExtended.Add(new CommentExtended()
                    {
                        Comment = comment,
                        User = context.Users.FirstOrDefault(p => p.UserID == comment.UserID)
                    });
                }
                return new ArticleExtended()
                {
                    Article = articleSelected,
                    CommentsExtended = commentsExtended,
                    User = context.Users.FirstOrDefault(p => p.UserID == articleSelected.UserID),
                    Category = category,
                    Tags = tags
                };
            }
            else
            {
                return null;
            }
        }

        public IList<Article> Articles(
            bool enabled = true,
            bool actual = true,
            int page = 0,
            int take = 0,
            int categoryID = 0,
            string tagName = null,
            int userID = 0,
            int bookmarkUserID = 0,
            string key = null,
            Expression<Func<Article, bool>> orderBy = null,
            Expression<Func<Article, bool>> orderByDescending = null)
        {
            IQueryable<Article> articles = context.Articles;

            if (enabled)
            {
                articles = articles.Where(p => p.Status == 1);
            }

            if (actual)
            {
                articles = articles.Where(p => p.DatePub < DateTime.Now && p.DateExp > DateTime.Now);
            }

            if (categoryID != 0)
            {
                articles = articles.Where(p => p.CategoryID == categoryID);
            }

            if (tagName != null)
            {
                Tag tag = context.Tags.Where(x => x.Name == tagName).FirstOrDefault();
                if (tag != null)
                {
                    var articleByTagIDs = context.ArticleTagAssociates.Where(p => p.TagID == tag.TagID).Select(p => p.ArticleID).ToList();
                    if (articleByTagIDs.Any())
                    {
                        articles = articles.Where(p => articleByTagIDs.Contains(p.ArticleID));
                    }
                }
            }

            if (bookmarkUserID != 0)
            {
                var articleByBookmarkIDs = context.BookmarkUserArticleAssociates.Where(p => p.UserID == bookmarkUserID).Select(p => p.ArticleID).ToList();
                if (articleByBookmarkIDs.Any())
                {
                    articles = articles.Where(p => articleByBookmarkIDs.Contains(p.ArticleID));
                }
                else
                {
                    return null;
                }
            }

            if (key != null)
            {
                articles = articles.Where(p => p.Content.Contains(key) || p.Title.Contains(key));
            }

            if (orderByDescending != null)
            {
                articles = articles.OrderByDescending(orderByDescending);
            }

            if (orderBy != null)
            {
                articles = articles.OrderBy(orderBy);
            }

            if (orderBy == null && orderByDescending == null)
            {
                articles = articles.OrderByDescending(p => p.DateAdd);
            }

            if (page != 0 && take != 0)
            {
                articles = articles.Skip((page - 1) * take);
            }

            if (take != 0)
            {
                articles = articles.Take(take);
            }

            return articles.ToList();
        }

        public IList<ArticleExtended> ArticlesExtended(
            bool enabled = true,
            bool actual = true,
            int page = 1,
            int take = 0,
            int categoryID = 0,
            string tagName = null,
            int userID = 0,
            int bookmarkUserID = 0,
            string key = null,
            Expression<Func<Article, bool>> orderBy = null,
            Expression<Func<Article, bool>> orderByDescending = null,
            IList<Article> articles = null)
        {
            IList<Article> articlesSelected = articles != null ? articles : Articles(enabled, actual, page, take, categoryID, tagName, userID, bookmarkUserID, key, orderBy, orderByDescending);
            IList<ArticleExtended> articlesExtended = new List<ArticleExtended>();
            foreach (Article article in articlesSelected)
            {
                articlesExtended.Add(ArticleExtended(article: article));
            }
            return articlesExtended;
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
                foreach (var bookmarkUserArticleAssociate in bookmarkUserArticleAssociates.ToList())
                {
                    bookmarkUserArticleAssociate.Status = 0;
                }
            }
            IQueryable<ArticleTagAssociate> articleTagAssociates = context.ArticleTagAssociates.Where(p => p.ArticleID == articleID);
            if (articleTagAssociates.Any())
            {
                foreach (var articleTagAssociate in articleTagAssociates.ToList())
                {
                    articleTagAssociate.Status = 0;
                }
            }
            IQueryable<Comment> comments = context.Comments.Where(p => p.ArticleID == articleID);
            if (comments.Any())
            {
                foreach (var comment in comments.ToList())
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

            IList<BookmarkUserArticleAssociate> bookmarkUserArticleAssociates = context.BookmarkUserArticleAssociates.Where(p => p.ArticleID == articleID).ToList();
            if (bookmarkUserArticleAssociates.Any())
            {
                foreach (var bookmarkUserArticleAssociate in bookmarkUserArticleAssociates)
                {
                    context.BookmarkUserArticleAssociates.Remove(bookmarkUserArticleAssociate);
                }
            }
            IList<ArticleTagAssociate> articleTagAssociates = context.ArticleTagAssociates.Where(p => p.ArticleID == articleID).ToList();
            if (articleTagAssociates.Any())
            {
                foreach (var articleTagAssociate in articleTagAssociates)
                {
                    context.ArticleTagAssociates.Remove(articleTagAssociate);
                }
            }

            IList<Comment> comments = context.Comments.Where(p => p.ArticleID == articleID).ToList();
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

                        IList<BookmarkUserArticleAssociate> bookmarkUserArticleAssociates = context.BookmarkUserArticleAssociates.Where(p => p.ArticleID == article.ArticleID).ToList();
                        if (bookmarkUserArticleAssociates.Any())
                        {
                            foreach (var bookmarkUserArticleAssociate in bookmarkUserArticleAssociates)
                            {
                                bookmarkUserArticleAssociate.Status = 1;
                            }
                        }
                        IList<ArticleTagAssociate> articleTagAssociates = context.ArticleTagAssociates.Where(p => p.ArticleID == article.ArticleID).ToList();
                        if (articleTagAssociates.Any())
                        {
                            foreach (var articleTagAssociate in articleTagAssociates)
                            {
                                articleTagAssociate.Status = 1;
                            }
                        }
                        IList<Comment> comments = context.Comments.Where(p => p.ArticleID == article.ArticleID).ToList();
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
            context.SaveChanges();
            return dbEntry.ArticleID;
        }

        public BookmarkUserArticleAssociate BookmarkUserArticleAssociate(
            int articleID = 0,
            int userID = 0)
        {
            IQueryable<BookmarkUserArticleAssociate> bookmarkUserArticleAssociates = context.BookmarkUserArticleAssociates;

            if (articleID != 0)
            {
                bookmarkUserArticleAssociates = bookmarkUserArticleAssociates.Where(p => p.ArticleID == articleID);
            }

            if (userID != 0)
            {
                bookmarkUserArticleAssociates = bookmarkUserArticleAssociates.Where(p => p.UserID == userID);
            }

            return bookmarkUserArticleAssociates.FirstOrDefault();
        }


        public void SaveBookmark(int articleID, int userID)
        {
            IList<BookmarkUserArticleAssociate> bookmarkUserArticleAssociates = context.BookmarkUserArticleAssociates.Where(p => p.ArticleID == articleID && p.UserID == userID).ToList();
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
            IList<BookmarkUserArticleAssociate> bookmarkUserArticleAssociates = context.BookmarkUserArticleAssociates.Where(p => p.ArticleID == articleID && p.UserID == userID).ToList();
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
            IList<ArticleTagAssociate> articleTagAssociates = context.ArticleTagAssociates.Where(p => p.ArticleID == articleID && p.TagID == tagID).ToList();
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

        public string GetUniqueArticleTitle(string title)
        {
            Article dbEntry = context.Articles.FirstOrDefault(p => p.Url == title);
            if (dbEntry != null)
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
            Article dbEntry = context.Articles.FirstOrDefault(p => p.ImageName == title);
            if (dbEntry != null)
            {
                string imageName = title.Substring(0, title.Length - 4);
                string ext = title.Substring(title.Length - 4);
                return GetUniqueImageTitle(textBuilder.RandomizeText(imageName) + ext);
            }
            else
            {
                return title;
            }
        }
    }
}