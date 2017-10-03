using LuzzedroCMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LuzzedroCMS.Domain.Abstract
{
    public interface ICategoryRepository
    {
        Category Category(
            bool enabled = true,
            int categoryID = 0,
            string categoryName = null);

        IList<Category> Categories(
            bool enabled = true,
            int page = 1,
            int take = 0,
            string categoryName = null,
            Expression<Func<Category, bool>> orderBy = null,
            Expression<Func<Category, bool>> orderByDescending = null);

        void Save(Category category);
        void Remove(int categoryID);
        void RemovePermanently(int categoryID);
    }
}