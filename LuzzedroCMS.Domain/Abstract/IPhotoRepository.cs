using LuzzedroCMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LuzzedroCMS.Domain.Abstract
{
    public interface IPhotoRepository
    {
        IList<Photo> Photos(
            bool enabled = true,
            int page = 1,
            int take = 0,
            int userID = 0,
            Expression<Func<Photo, bool>> orderBy = null,
            Expression<Func<Photo, bool>> orderByDescending = null);

        void Save(Photo photo, int userId);
        void Remove(int photoID);
        void RemovePermanently(int photoID);
    }
}