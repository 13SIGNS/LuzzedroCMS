using LuzzedroCMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuzzedroCMS.Domain.Abstract
{
    public interface IUserRepository
    {
        User UserByID(int userID);
        User UserByEmail(string email);
        User UserByNick(string nick);
        User UserByToken(string token);
        IQueryable<User> UsersEnabled { get; }
        IQueryable<User> UsersTotal { get; }
        UserTemp SaveUserTemp(UserTemp userTemp);
        UserToken SaveUserToken(UserToken userToken);
        UserToken UserTokenByToken(string token);
        User TransferUserTemp(string token);
        UserTemp UserTempByToken(string token);
        string GetUniqueImageTitle(string photoUrl);
        void RemoveUserToken(string token);
        void Save(User user);
        void Remove(int userID);
        void RemovePermanently(int userID);
    }
}