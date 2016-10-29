using LuzzedroCMS.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LuzzedroCMS.Domain.Entities;

namespace LuzzedroCMS.Domain.Concrete
{
    public class EFPhotoRepository : IPhotoRepository
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<Photo> PhotosEnabled
        {
            get
            {
                return context.Photos.Where(p => p.Status == 1);
            }
        }

        public IQueryable<Photo> PhotosTotal
        {
            get
            {
                return context.Photos;
            }
        }

        public IQueryable<Photo> PhotosEnabledByUserID(int userID)
        {
            IQueryable<Photo> photos = null;
            IQueryable<UserPhotoAssociate> userPhotoAssociates = context.UserPhotoAssociates.Where(p => p.UserID == userID);
            foreach(var userPhotoAssociate in userPhotoAssociates)
            {
                photos.AsEnumerable().Concat(context.Photos.Where(p => p.PhotoID == userPhotoAssociate.PhotoID && p.Status == 1));
            }
            return photos;
        }

        public void Remove(int photoID)
        {
            Photo photo = context.Photos.Find(photoID);
            if (photo != null)
            {
                photo.Status = 0;
            }
            IQueryable<UserPhotoAssociate> userPhotoAssociates = context.UserPhotoAssociates.Where(p => p.PhotoID == photoID);
            if (userPhotoAssociates != null)
            {
                foreach (var userPhotoAssociate in userPhotoAssociates)
                {
                    userPhotoAssociate.Status = 0;
                }
            }
            context.SaveChanges();
        }

        public void RemovePermanently(int photoID)
        {
            Photo photo = context.Photos.Find(photoID);
            if (photo != null)
            {
                context.Photos.Remove(photo);
            }
            IQueryable<UserPhotoAssociate> userPhotoAssociates = context.UserPhotoAssociates.Where(p => p.PhotoID == photoID);
            if (userPhotoAssociates != null)
            {
                foreach (var userPhotoAssociate in userPhotoAssociates)
                {
                    context.UserPhotoAssociates.Remove(userPhotoAssociate);
                }
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
                    if (dbEntry.Status == 0 && photo.Status == 1)
                    {
                        IQueryable<UserPhotoAssociate> userPhotoAssociates = context.UserPhotoAssociates.Where(p => p.PhotoID == photo.PhotoID);
                        if (userPhotoAssociates != null)
                        {
                            foreach (var userPhotoAssociate in userPhotoAssociates)
                            {
                                userPhotoAssociate.Status = 1;
                            }
                        } 
                    }
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