using LuzzedroCMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LuzzedroCMS.Domain.Abstract
{
    public interface ITagRepository
    {
        Tag Tag(
            bool enabled = true,
            string name = null,
            int tagID = 0);

        IList<Tag> Tags(
            bool enabled = true,
            int page = 1,
            int take = 0,
            int articleID = 0,
            Expression<Func<Tag, bool>> orderBy = null,
            Expression<Func<Tag, bool>> orderByDescending = null);

        IDictionary<string, int> TagsCounted(
            bool enabled = true,
            int page = 1,
            int take = 0,
            int articleID = 0,
            Expression<Func<Tag, bool>> orderBy = null,
            Expression<Func<Tag, bool>> orderByDescending = null);

        void Save(Tag tag);
        void Remove(int tagID);
        void RemovePermanently(int tagID);
    }
}