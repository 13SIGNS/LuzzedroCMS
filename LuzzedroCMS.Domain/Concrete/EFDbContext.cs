using LuzzedroCMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LuzzedroCMS.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentParentAssociate> CommentParentAssociates { get; set; }
        public DbSet<ArticleCommentAssociate> ArticleCommentAssociates { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserTemp> UserTemps { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ArticleTagAssociate> ArticleTagAssociates { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ArticleCategoryAssociate> ArticleCategoryAssociates { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<BookmarkUserArticleAssociate> BookmarkUserArticleAssociates { get; set; }
        public DbSet<UserPhotoAssociate> UserPhotoAssociates { get; set; }
        public DbSet<UserRoleAssociate> UserRoleAssociates { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
    }
}