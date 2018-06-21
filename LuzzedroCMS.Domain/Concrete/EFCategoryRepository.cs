using LuzzedroCMS.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LuzzedroCMS.Domain.Entities;
using System.Linq.Expressions;

namespace LuzzedroCMS.Domain.Concrete
{
    public class EFCategoryRepository : ICategoryRepository
    {
        private EFDbContext context = new EFDbContext();

        public Category Category(
            bool enabled = true,
            int categoryID = 0,
            string categoryName = null)
        {
            IQueryable<Category> categories = context.Categories;

            if (enabled)
            {
                categories = categories.Where(p => p.Status == 1);
            }

            if (categoryID != 0)
            {
                categories = categories.Where(p => p.CategoryID == categoryID);
            }

            if (categoryName != null)
            {
                categories = categories.Where(p => p.Name == categoryName);
            }

            return categories.FirstOrDefault();
        }

        public IList<Category> Categories(
            bool enabled = true,
            int page = 1,
            int take = 0,
            string categoryName = null,
            Expression<Func<Category, bool>> orderBy = null,
            Expression<Func<Category, bool>> orderByDescending = null)
        {
            IQueryable<Category> categories = context.Categories;

            if (enabled)
            {
                categories = categories.Where(p => p.Status == 1);
            }

            if (categoryName != null)
            {
                categories = categories.Where(p => p.Name == categoryName);
            }

            if (orderByDescending != null)
            {
                categories = categories.OrderByDescending(orderByDescending);
            }

            if (orderBy != null)
            {
                categories = categories.OrderBy(orderBy);
            }

            if (orderBy == null && orderByDescending == null)
            {
                categories = categories.OrderByDescending(p => p.Order);
            }

            if (page != 0 && take != 0)
            {
                categories = categories.Skip((page - 1) * take);
            }

            if (take != 0)
            {
                categories = categories.Take(take);
            }

            return categories.ToList();
        }

        public void Remove(int categoryID)
        {
            Category dbEntry = context.Categories.Find(categoryID);
            if (dbEntry != null)
            {
                dbEntry.Status = 0;

            }
            context.SaveChanges();
        }

        public void RemovePermanently(int categoryID)
        {
            Category dbEntry = context.Categories.Find(categoryID);
            if (dbEntry != null)
            {
                context.Categories.Remove(dbEntry);
            }

            context.SaveChanges();
        }

        public void Save(Category category)
        {
            if (category.CategoryID == 0)
            {
                IQueryable<Category> existingCategory = context.Categories.Where(p => p.Name == category.Name);
                if (!existingCategory.Any())
                {
                    context.Categories.Add(new Category
                    {
                        Name = category.Name,
                        Order = category.Order,
                        Status = 1
                    });
                }
            }
            else
            {
                Category dbEntry = context.Categories.Find(category.CategoryID);
                if (dbEntry != null)
                {
                    dbEntry.Name = category.Name;
                    dbEntry.Order = category.Order;
                    dbEntry.Status = category.Status;
                }
            }
            context.SaveChanges();
        }
    }
}