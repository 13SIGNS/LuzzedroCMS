using LuzzedroCMS.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LuzzedroCMS.Domain.Entities;

namespace LuzzedroCMS.Domain.Concrete
{
    public class EFRoleRepository : IRoleRepository
    {
        private EFDbContext context = new EFDbContext();

        public int RoleIdByName(string roleName)
        {
            Role roleId = context.Roles.FirstOrDefault(p => p.Name == roleName);
            return roleId.RoleID;
        }

        public string[] RolesByUserEmail(string email)
        {
            int userID = context.Users.Where(p => p.Email == email).Select(p => p.UserID).FirstOrDefault();
            IList<UserRoleAssociate> roles = context.UserRoleAssociates.Where(p => p.UserID == userID).ToList();
            if (roles.Any())
            {
                string[] roleTable = new string[roles.Count()];
                int counter = 0;
                var contextRoles = context.Roles;
                foreach (var role in roles)
                {
                    roleTable[counter] = contextRoles.Where(p => p.RoleID == role.RoleID).Select(p => p.Name).FirstOrDefault();

                    counter++;
                }

                return roleTable;
            }
            else
            {
                return null;
            }
        }

        public void SaveUserRole(int userID, int roleID)
        {
            context.UserRoleAssociates.Add(new UserRoleAssociate
            {
                UserID = userID,
                RoleID = roleID
            });
            context.SaveChanges();
        }
    }
}