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
                    //articles = articles.Where(p => p.ArticleID == comment.ArticleID);
                }
            }

            if (articleID != 0)
            {
                articles = articles.Where(p => p.ArticleID == articleID);
            }

            return articles.FirstOrDefault();
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
                articles = articles.Where(p => p.Category.CategoryID == categoryID);
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
                articles = articles.OrderByDescending(p => p.DatePub);
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

        public void Remove(int articleID)
        {
            Article article = context.Articles.Find(articleID);
            if (article != null)
            {
                article.Status = 0;
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
                    Category = article.Category,
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
                    dbEntry.UserID = article.UserID;
                    dbEntry.Category = article.Category;
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