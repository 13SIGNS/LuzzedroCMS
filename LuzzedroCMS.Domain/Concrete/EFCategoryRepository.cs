using LuzzedroCMS.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LuzzedroCMS.Domain.Entities;

namespace LuzzedroCMS.Domain.Concrete
{
    public class EFCategoryRepository : ICategoryRepository
    {
        private EFDbContext context = new EFDbContext();

        public Category CategoryByID(int categoryID)
        {
            return context.Categories.Find(categoryID);
        }

        public Category CategoryByName(string categoryName)
        {
            return context.Categories.Where(x => x.Name ==  categoryName).FirstOrDefault();
        }

        public IQueryable<Category> CategoriesTotal
        {
            get
            {
                return context.Categories.OrderBy(x => x.Order);
            }
        }

        public IQueryable<Category> CategoriesEnabled
        {
            get
            {
                return context.Categories.Where(p => p.Status == 1).OrderBy(x => x.Order); ;
            }
        }

        

        public void Remove(int categoryID)
        {
            Category dbEntry = context.Categories.Find(categoryID);
            if (dbEntry != null)
            {
                dbEntry.Status = 0;
                
            }
            IQueryable<ArticleCategoryAssociate> dbEntryAssociates = context.ArticleCategoryAssociates.Where(p => p.CategoryID == categoryID);
            foreach (var dbEntryAssociate in dbEntryAssociates)
            {
                if (dbEntryAssociate != null)
                {
                    dbEntryAssociate.Status = 0;
                }
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
            IQueryable<ArticleCategoryAssociate> dbEntryAssociates = context.ArticleCategoryAssociates.Where(p => p.CategoryID == categoryID);
            foreach (var dbEntryAssociate in dbEntryAssociates)
            {
                if (dbEntryAssociate != null)
                {
                    context.ArticleCategoryAssociates.Remove(dbEntryAssociate);
                }
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