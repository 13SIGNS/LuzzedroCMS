using LuzzedroCMS.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LuzzedroCMS.Domain.Entities;
using System.Linq.Expressions;

namespace LuzzedroCMS.Domain.Concrete
{
    public class EFPhotoRepository : IPhotoRepository
    {
        private EFDbContext context = new EFDbContext();

        public IList<Photo> Photos(
            bool enabled = true,
            int page = 1,
            int take = 0,
            int userID = 0,
            Expression<Func<Photo, bool>> orderBy = null,
            Expression<Func<Photo, bool>> orderByDescending = null)
        {
            IQueryable<Photo> photos = context.Photos;

            if (enabled)
            {
                photos = photos.Where(p => p.Status == 1);
            }

            if (orderByDescending != null)
            {
                photos = photos.OrderByDescending(orderByDescending);
            }

            if (orderBy != null)
            {
                photos = photos.OrderBy(orderBy);
            }

            if (orderBy == null && orderByDescending == null)
            {
                photos = photos.OrderByDescending(p => p.Date);
            }

            if (page != 0 && take != 0)
            {
                photos = photos.Skip((page - 1) * take);
            }

            if (take != 0)
            {
                photos = photos.Take(take);
            }

            return photos.ToList();
        }

        public void RemovePermanently(int photoID)
        {
            Photo photo = context.Photos.Find(photoID);
            if (photo != null)
            {
                context.Photos.Remove(photo);
            }

            context.SaveChanges();
        }

        public void Save(Photo photo, int userId)
        {
            if (photo.PhotoID == 0)
            {
                context.Photos.Add(new Photo
                {
                    Date = DateTime.Now,
                    Name = photo.Name,
                    Desc = photo.Desc,
                    Status = photo.Status
                });
            }
            else
            {
                Photo dbEntry = context.Photos.Find(photo.PhotoID);
                if (dbEntry != null)
                {
                    dbEntry.Date = DateTime.Now;
                    dbEntry.Name = photo.Name;
                    dbEntry.Desc = photo.Desc;
                    dbEntry.Status = 1;
                }
            }
            context.SaveChanges();
        }
    }
}