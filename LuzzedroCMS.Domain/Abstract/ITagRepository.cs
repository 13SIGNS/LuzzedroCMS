using LuzzedroCMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuzzedroCMS.Domain.Abstract
{
    public interface ITagRepository
    {
        Tag TagByName(string name);
        Tag TagByID(int tagID);
        IQueryable<Tag> TagsEnabled { get; }
        IQueryable<Tag> TagsTotal { get; }
        IQueryable<Tag> TagsEnabledByArticleID(int articleID);
        IQueryable<int> TagIDsEnabledByArticleID(int articleID);
        IDictionary<string, int> TagsEnabledByAssociate { get; }
        void Save(Tag tag);
        void Remove(int tagID);
        void RemovePermanently(int tagID);
    }
}