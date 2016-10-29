using LuzzedroCMS.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LuzzedroCMS.Domain.Entities;
using LuzzedroCMS.Domain.Infrastructure;

namespace LuzzedroCMS.Domain.Concrete
{
    public class EFUserRepository : IUserRepository
    {
        private EFDbContext context = new EFDbContext();
        private TextBuilder textBuilder = new TextBuilder();

        public User UserByID(int userID)
        {
            return context.Users.Find(userID);
        }

        public User UserByEmail(string email)
        {
            return context.Users.Where(p => p.Email == email).FirstOrDefault();
        }

        public User UserByNick(string nick)
        {
            return context.Users.Where(p => p.Nick == nick).FirstOrDefault();
        }

        public User UserByToken(string token)
        {
            int userID = context.UserTokens.Where(p => p.Token == token).Select(p => p.UserID).FirstOrDefault();
            return UserByID(userID);
        }

        public IQueryable<User> UsersEnabled
        {
            get
            {
                return context.Users.Where(p => p.Status == 1);
            }
        }

        public IQueryable<User> UsersTotal
        {
            get
            {
                return context.Users;
            }
        }

        public void Remove(int userID)
        {
            User user = context.Users.Find(userID);
            if (user != null)
            {
                user.Status = 0;
            }
            context.SaveChanges();
            IQueryable<UserPhotoAssociate> userPhotoAssociates = context.UserPhotoAssociates.Where(p => p.UserID == userID);
            if (userPhotoAssociates != null)
            {
                foreach (var userPhotoAssociate in userPhotoAssociates)
                {
                    userPhotoAssociate.Status = 0;
                }
            }
        }

        public void RemovePermanently(int userID)
        {
            User user = context.Users.Find(userID);
            if (user != null)
            {
                context.Users.Remove(user);
            }
            IQueryable<UserPhotoAssociate> userPhotoAssociates = context.UserPhotoAssociates.Where(p => p.UserID == userID);
            if (userPhotoAssociates != null)
            {
                foreach (var userPhotoAssociate in userPhotoAssociates)
                {
                    context.UserPhotoAssociates.Remove(userPhotoAssociate);
                }
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
                    Status = user.Status
                });
                //User User = context.Users.Where(p => p.Email == newUser.Email).FirstOrDefault();
                //context.UserRoleAssociates.Add(new UserRoleAssociate
                //{
                //    UserID = User.UserID,
                //    RoleID = 2
                //});
                context.SaveChanges();
            }
            else
            {
                User dbEntry = context.Users.Find(user.UserID);
                if (dbEntry != null)
                {
                    dbEntry.Date = DateTime.Now;
                    dbEntry.Email = user.Email;
                    dbEntry.Nick = user.Nick;
                    dbEntry.Password = user.Password;
                    dbEntry.PhotoUrl = user.PhotoUrl;
                    dbEntry.Status = user.Status;
                }
            }
            context.SaveChanges();
        }

        public UserTemp SaveUserTemp(UserTemp userTemp)
        {
            UserTemp result = null;
            try
            {
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
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Exception ex = e.InnerException;
                result = null;
            }
            return result;
        }

        public UserTemp UserTempByToken(string token)
        {
            UserTemp dbEntry = context.UserTemps.Where(p => p.Token == token).FirstOrDefault();
            return dbEntry;
        }

        public User TransferUserTemp(string token)
        {
            User newUser = null;
            UserTemp userTemp = context.UserTemps.Where(p => p.Token == token).FirstOrDefault();
            if (userTemp != null)
            {
                newUser = new User
                {
                    Date = DateTime.Now,
                    Email = userTemp.Email,
                    Nick = userTemp.Nick,
                    Password = userTemp.Password,
                    Status = 1
                };
                this.Save(newUser);
                context.UserTemps.Remove(userTemp);
            }
            return newUser;
        }

        public UserToken SaveUserToken(UserToken userToken)
        {
            UserToken result = null;
            try
            {
                UserToken newdbEntry = new UserToken
                {
                    UserID = userToken.UserID,
                    Token = userToken.Token,
                    ExpiryDate = userToken.ExpiryDate
                };
                context.UserTokens.Add(newdbEntry);
                result = newdbEntry;
                context.SaveChanges();
            }
            catch (Exception e)
            {
                Exception ex = e.InnerException;
                result = null;
            }
            return result;
        }

        public void RemoveUserToken(string token)
        {
            IQueryable<UserToken> userTokens = context.UserTokens.Where(p => p.Token == token);
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
            if(photoUrl == "")
            {
                photoUrl = textBuilder.GetRandomString(60) + ".jpg";
            }
            int dbEntry = context.Users.Where(p => p.PhotoUrl == photoUrl).Select(p => p.UserID).FirstOrDefault();
            if (dbEntry != 0)
            {
                return GetUniqueImageTitle(textBuilder.GetRandomString(60)+".jpg");
            }
            else
            {
                return photoUrl;
            }
        }
    }
}