using LuzzedroCMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuzzedroCMS.Domain.Abstract
{
    public interface IPhotoRepository
    {
        IQueryable<Photo> PhotosEnabled { get; }
        IQueryable<Photo> PhotosTotal { get; }
        IQueryable<Photo> PhotosEnabledByUserID(int userID);
        void Save(Photo photo, int userId);
        void Remove(int photoID);
        void RemovePermanently(int photoID);
    }
}