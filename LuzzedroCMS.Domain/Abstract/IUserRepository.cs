using LuzzedroCMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LuzzedroCMS.Domain.Abstract
{
    public interface IUserRepository
    {
        User User(
            bool enabled = true,
            int userID = 0,
            string email = null,
            string nick = null,
            string token = null);

        IList<User> Users(
            bool enabled = true,
            int page = 1,
            int take = 0,
            Expression<Func<User, bool>> orderBy = null,
            Expression<Func<User, bool>> orderByDescending = null);

        UserTemp SaveUserTemp(UserTemp userTemp);
        UserToken SaveUserToken(UserToken userToken);
        UserToken UserTokenByToken(string token);
        User TransferUserTemp(string token, int source = 0);
        UserTemp UserTempByToken(string token);
        string GetUniqueImageTitle(string photoUrl);
        void RemoveUserToken(string token);
        void Save(User user);
        void Remove(int userID);
        void RemovePermanently(int userID);
    }
}