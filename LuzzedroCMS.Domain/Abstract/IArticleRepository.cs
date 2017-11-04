using LuzzedroCMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LuzzedroCMS.Domain.Abstract
{
    public interface IArticleRepository
    {
        Article Article(
            bool enabled = true,
            bool actual = true,
            string url = null,
            int commentID = 0,
            int articleID = 0);

        ArticleExtended ArticleExtended(
            bool enabled = true,
            bool actual = true,
            string url = null,
            int commentID = 0,
            int articleID = 0,
            bool commentEnabled = true,
            Article article = null);

        IList<Article> Articles(
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
            Expression<Func<Article, bool>> orderByDescending = null);

        IList<ArticleExtended> ArticlesExtended(
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
            IList<Article> articles = null);

        BookmarkUserArticleAssociate BookmarkUserArticleAssociate(
            int articleID = 0,
            int userID = 0);

        void SaveBookmark(int articleID, int userID);
        void RemoveBookmark(int articleID, int userID);
        void AddTagToArticle(int articleID, int tagID);
        void RemoveTagFromArticle(int articleID, int tagID);
        void RemoveAllTagsFromArticle(int articleID);
        int Save(Article article);
        void Enable(int articleID);
        void Remove(int articleID);
        void RemovePermanently(int articleID);
        string GetUniqueArticleTitle(string title);
        string GetUniqueImageTitle(string title);
    }
}