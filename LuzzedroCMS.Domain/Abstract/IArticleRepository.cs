using LuzzedroCMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuzzedroCMS.Domain.Abstract
{
    public interface IArticleRepository
    {
        Article ArticleByUrl(string url);
        IQueryable<Article> Articles { get; }
        IQueryable<Article> ArticlesEnabled { get; }
        IQueryable<Article> ArticlesEnabledActual { get; }
        IQueryable<Article> ArticlesTotal { get; }
        IQueryable<Article> ArticlesEnabledActualByCategoryID(int categoryID);
        IQueryable<Article> ArticlesEnabledActualByTagName(string tagName);
        IQueryable<Article> ArticlesEnabledActualByUserID(int userID);
        IQueryable<Article> ArticlesEnabledActualByBookmark(int userID);
        IQueryable<Article> ArticlesEnabledActualByKey(string key);
        Article ArticleEnabledActualByComment(int commentID);
        void SaveBookmark(int articleID, int userID);
        void RemoveBookmark(int articleID, int userID);
        void AddTagToArticle(int articleID, int tagID);
        void RemoveTagFromArticle(int articleID, int tagID);
        int Save(Article article);
        void Enable(int articleID);
        void Remove(int articleID);
        void RemovePermanently(int articleID);
        string GetUniqueArticleTitle(string title);
        string GetUniqueImageTitle(string title);
    }
}