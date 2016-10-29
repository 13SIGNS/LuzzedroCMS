using LuzzedroCMS.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LuzzedroCMS.Domain.Entities;

namespace LuzzedroCMS.Domain.Concrete
{
    public class EFTagRepository : ITagRepository
    {
        private EFDbContext context = new EFDbContext();

        public Tag TagByName(string name)
        {
            return context.Tags.Where(p => p.Name == name).FirstOrDefault();
        }
        public Tag TagByID(int tagID)
        {
            return context.Tags.Find(tagID);
        }

        public IQueryable<Tag> TagsEnabled
        {
            get
            {
                return context.Tags.Where(p => p.Status == 1);
            }
        }

        public IDictionary<string, int> TagsEnabledByAssociate
        {
            get
            {
                IDictionary<string, int> tags = new Dictionary<string, int>();
                IQueryable<int> tagIDs = context.ArticleTagAssociates.Select(x => x.TagID);
                foreach (var tagID in tagIDs)
                {
                    string tagName = context.Tags.Where(x => x.TagID == tagID).Select(x => x.Name).FirstOrDefault();
                    int tagCount = context.ArticleTagAssociates.Where(x => x.TagID== tagID).Select(x => x.TagID).Count();
                    if (!tags.ContainsKey(tagName))
                    {
                        tags.Add(tagName, tagCount);
                    }
                }
                return tags;
            }
        }

        public IQueryable<Tag> TagsTotal
        {
            get
            {
                return context.Tags;
            }
        }

        public void Remove(int tagID)
        {
            Tag tag = context.Tags.Find(tagID);
            if (tag != null)
            {
                tag.Status = 0;
            }
            context.SaveChanges();
        }

        public void RemovePermanently(int tagID)
        {
            Tag tag = context.Tags.Find(tagID);
            if (tag != null)
            {
                context.Tags.Remove(tag);
            }
            context.SaveChanges();
        }

        public void Save(Tag tag)
        {
            if (tag.TagID == 0)
            {
                context.Tags.Add(new Tag
                {
                    Name = tag.Name,
                    Status = tag.Status
                });
            }
            else
            {
                Tag dbEntry = context.Tags.Find(tag.TagID);
                if (dbEntry != null)
                {
                    dbEntry.Name = tag.Name;
                    dbEntry.Status = 1;
                }
            }
            context.SaveChanges();
        }

        public IQueryable<Tag> TagsEnabledByArticleID(int articleID)
        {
            IQueryable<Tag> tags = null;
            IQueryable<ArticleTagAssociate> articleTagAssociates = context.ArticleTagAssociates.Where(p => p.ArticleID == articleID && p.Status == 1);
            foreach (var articleTagAssociate in articleTagAssociates)
            {
                tags.AsEnumerable().Concat(context.Tags.Where(p => p.TagID == articleTagAssociate.TagID && p.Status == 1));
            }
            return tags;
        }

        public IQueryable<int> TagIDsEnabledByArticleID(int articleID)
        {
            List<int> tagsList = new List<int>();
            IQueryable<ArticleTagAssociate> articleTagAssociates = context.ArticleTagAssociates.Where(p => p.ArticleID == articleID && p.Status == 1);
            foreach (var articleTagAssociate in articleTagAssociates)
            {
                IQueryable<Tag> tags = context.Tags.Where(p => p.TagID == articleTagAssociate.TagID && p.Status == 1);
                foreach (Tag tag in tags)
                {
                    tagsList.Add(tag.TagID);
                }
            }
            return tagsList.AsQueryable();
        }
    }
}