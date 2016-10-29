using LuzzedroCMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuzzedroCMS.Domain.Abstract
{
    public interface ICategoryRepository
    {
        Category CategoryByID(int categoryID);
        Category CategoryByName(string categoryName);
        IQueryable<Category> CategoriesEnabled { get; }
        IQueryable<Category> CategoriesTotal { get; }
        void Save(Category category);
        void Remove(int categoryID);
        void RemovePermanently(int categoryID);
    }
}