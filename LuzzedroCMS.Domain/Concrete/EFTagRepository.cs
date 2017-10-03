using LuzzedroCMS.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LuzzedroCMS.Domain.Entities;
using System.Linq.Expressions;

namespace LuzzedroCMS.Domain.Concrete
{
    public class EFTagRepository : ITagRepository
    {
        private EFDbContext context = new EFDbContext();

        public Tag Tag(
            bool enabled = true,
            string name = null,
            int tagID = 0)
        {
            IQueryable<Tag> tags = context.Tags;

            if (enabled)
            {
                tags = tags.Where(p => p.Status == 1);
            }

            if (name != null)
            {
                tags = tags.Where(p => p.Name == name);
            }

            if (tagID != 0)
            {
                tags = tags.Where(p => p.TagID == tagID);
            }

            return tags.FirstOrDefault();
        }

        public IList<Tag> Tags(
            bool enabled = true,
            int page = 1,
            int take = 0,
            int articleID = 0,
            Expression<Func<Tag, bool>> orderBy = null,
            Expression<Func<Tag, bool>> orderByDescending = null
            )
        {
            IQueryable<Tag> tags = context.Tags;

            if (enabled)
            {
                tags = tags.Where(p => p.Status == 1);
            }

            if (articleID != 0)
            {
                var tagIDs = context.ArticleTagAssociates.Where(p => p.ArticleID == articleID).Select(x => x.TagID).ToList();
                if (tagIDs != null)
                {
                    tags = tags.Where(p => tagIDs.Contains(p.TagID));
                }
            }

            if (orderByDescending != null)
            {
                tags = tags.OrderByDescending(orderByDescending);
            }

            if (orderBy != null)
            {
                tags = tags.OrderBy(orderBy);
            }

            if (orderBy == null && orderByDescending == null)
            {
                tags = tags.OrderByDescending(p => p.TagID);
            }

            if (page != 0 && take != 0)
            {
                tags = tags.Skip((page - 1) * take);
            }

            if (take != 0)
            {
                tags = tags.Take(take);
            }

            return tags.ToList();
        }

        public IDictionary<string, int> TagsCounted(
            bool enabled = true,
            int page = 1,
            int take = 0,
            int articleID = 0,
            Expression<Func<Tag, bool>> orderBy = null,
            Expression<Func<Tag, bool>> orderByDescending = null)
        {
            IList<Tag> tags = Tags(enabled, page, take, articleID, orderBy, orderByDescending);
            IDictionary<string, int> tagsCounted = new Dictionary<string, int>();

            foreach (var tag in tags)
            {
                string tagName = tag.Name;
                int tagCount = context.ArticleTagAssociates.Where(x => x.TagID == tag.TagID).Select(x => x.TagID).Count();
                if (tagCount != 0)
                {
                    if (!tagsCounted.ContainsKey(tagName))
                    {
                        tagsCounted.Add(tagName, tagCount);
                    }
                }
            }
            return tagsCounted;
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
    }
}