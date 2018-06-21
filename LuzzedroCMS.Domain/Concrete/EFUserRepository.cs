using LuzzedroCMS.Domain.Abstract;
using LuzzedroCMS.Domain.Entities;
using LuzzedroCMS.Domain.Infrastructure.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LuzzedroCMS.Domain.Concrete
{
    public class EFUserRepository : IUserRepository
    {
        private EFDbContext context = new EFDbContext();
        private TextBuilder textBuilder = new TextBuilder();

        public User User(
            bool enabled = true,
            int userID = 0,
            string email = null,
            string nick = null,
            string token = null)
        {
            IQueryable<User> users = context.Users;

            if (enabled)
            {
                users = users.Where(p => p.Status == 1);
            }

            if (userID != 0)
            {
                users = users.Where(p => p.UserID == userID);
            }

            if (email != null)
            {
                users = users.Where(p => p.Email == email);
            }

            if (nick != null)
            {
                users = users.Where(p => p.Nick == nick);
            }

            if (token != null)
            {
                UserToken userIDfromToken = context.UserTokens.FirstOrDefault(p => p.Token == token);
                users = users.Where(p => p.UserID == userIDfromToken.UserID);
            }

            return users.FirstOrDefault();
        }

        public IList<User> Users(
            bool enabled = true,
            int page = 1,
            int take = 0,
            Expression<Func<User, bool>> orderBy = null,
            Expression<Func<User, bool>> orderByDescending = null)
        {
            IQueryable<User> users = context.Users;

            if (enabled)
            {
                users = users.Where(p => p.Status == 1);
            }

            if (orderByDescending != null)
            {
                users = users.OrderByDescending(orderByDescending);
            }

            if (orderBy != null)
            {
                users = users.OrderBy(orderBy);
            }

            if (orderBy == null && orderByDescending == null)
            {
                users = users.OrderByDescending(p => p.Date);
            }

            if (page != 0 && take != 0)
            {
                users = users.Skip((page - 1) * take);
            }

            if (take != 0)
            {
                users = users.Take(take);
            }

            return users.ToList();
        }

        public void Remove(int userID)
        {
            User user = context.Users.Find(userID);
            if (user != null)
            {
                user.Status = 0;
            }
            context.SaveChanges();
        }

        public void RemovePermanently(int userID)
        {
            User user = context.Users.Find(userID);
            if (user != null)
            {
                context.Users.Remove(user);
            }

            context.SaveChanges();
        }

        public void Save(User user)
        {
            if (user.UserID == 0)
            {
                User newUser = context.Users.Add(new User
                {
                    Date = DateTime.Now,
                    Email = user.Email,
                    Nick = user.Nick,
                    Password = user.Password,
                    PhotoUrl = user.PhotoUrl,
                    Status = user.Status,
                    Source = user.Source
                });
                context.SaveChanges();
                User userOld = context.Users.Where(p => p.Email == newUser.Email).FirstOrDefault();
                if (userOld != null)
                {
                    context.UserRoleAssociates.Add(new UserRoleAssociate
                    {
                        UserID = userOld.UserID,
                        RoleID = 2
                    });
                }
            }
            else
            {
                User dbEntry = context.Users.Find(user.UserID);
                if (dbEntry != null)
                {
                    dbEntry.Email = user.Email;
                    dbEntry.Nick = !string.IsNullOrEmpty(user.Nick) ? user.Nick : null;
                    dbEntry.Password = user.Password;
                    dbEntry.PhotoUrl = user.PhotoUrl;
                    dbEntry.Status = user.Status;
                    dbEntry.Source = user.Source;
                }
            }
            context.SaveChanges();
        }

        public UserTemp SaveUserTemp(UserTemp userTemp)
        {
            UserTemp result = null;
            UserTemp dbEntry = context.UserTemps.Where(p => p.Email == userTemp.Email).FirstOrDefault();
            if (dbEntry != null)
            {
                dbEntry.Date = DateTime.Now;
                dbEntry.Email = userTemp.Email;
                dbEntry.Nick = userTemp.Nick;
                dbEntry.Password = userTemp.Password;
                dbEntry.Token = userTemp.Token;
                result = dbEntry;
            }
            else
            {
                UserTemp newdbEntry = new UserTemp
                {
                    Date = DateTime.Now,
                    Email = userTemp.Email,
                    Nick = userTemp.Nick,
                    Password = userTemp.Password,
                    Token = userTemp.Token
                };
                context.UserTemps.Add(newdbEntry);
                result = newdbEntry;
            }
            context.SaveChanges();
            return result;
        }

        public UserTemp UserTempByToken(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                UserTemp dbEntry = context.UserTemps.Where(p => p.Token == token).FirstOrDefault();
                return dbEntry;
            }
            else
            {
                return null;
            }
        }

        public User TransferUserTemp(string token, int source = 0)
        {
            User user = null;
            UserTemp userTemp = context.UserTemps.Where(p => p.Token == token).FirstOrDefault();
            if (userTemp != null)
            {
                user = context.Users.FirstOrDefault(p => p.Email == userTemp.Email);
                if (user != null)
                {
                    user.Password = userTemp.Password;
                    user.Source = 0;
                    this.Save(user);
                }
                else
                {
                    user = new User
                    {
                        Date = DateTime.Now,
                        Email = userTemp.Email,
                        Nick = userTemp.Nick,
                        Password = userTemp.Password,
                        Status = 1,
                        Source = source
                    };
                    this.Save(user);
                    context.UserTemps.Remove(userTemp);
                    context.SaveChanges();
                }
            }
            return user;
        }

        public UserToken SaveUserToken(UserToken userToken)
        {
            UserToken result = null;
            UserToken newdbEntry = new UserToken
            {
                UserID = userToken.UserID,
                Token = userToken.Token,
                ExpiryDate = userToken.ExpiryDate
            };
            context.UserTokens.Add(newdbEntry);
            result = newdbEntry;
            context.SaveChanges();
            return result;
        }

        public void RemoveUserToken(string token)
        {
            IList<UserToken> userTokens = context.UserTokens.Where(p => p.Token == token).ToList();
            if (userTokens != null)
            {
                foreach (var userToken in userTokens)
                {
                    context.UserTokens.Remove(userToken);
                }

                context.SaveChanges();
            }
        }

        public UserToken UserTokenByToken(string token)
        {
            return context.UserTokens.Where(p => p.Token == token).FirstOrDefault();
        }

        public string GetUniqueImageTitle(string photoUrl = "")
        {
            if (photoUrl == string.Empty)
            {
                photoUrl = textBuilder.GetRandomString(60) + ".jpg";
            }

            int dbEntry = context.Users.Where(p => p.PhotoUrl == photoUrl).Select(p => p.UserID).FirstOrDefault();
            if (dbEntry != 0)
            {
                return GetUniqueImageTitle(textBuilder.GetRandomString(60) + ".jpg");
            }
            else
            {
                return photoUrl;
            }
        }
    }
}