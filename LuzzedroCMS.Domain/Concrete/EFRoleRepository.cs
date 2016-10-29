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
            int roleId = context.Roles.Where(p => p.Name == roleName).Select(p => p.RoleID).FirstOrDefault();
            return roleId;
        }

        public string[] RolesByUserEmail(string email)
        {
            int userID = context.Users.Where(p => p.Email == email).Select(p => p.UserID).FirstOrDefault();
            IQueryable<UserRoleAssociate> roles = context.UserRoleAssociates.Where(p => p.UserID == userID);
            string[] roleTable = new string[roles.Count()];
            int counter = 0;
            var contextRoles = context.Roles;
            foreach (var role in roles)
            {
                roleTable[counter] = contextRoles.Where(p => p.RoleID == role.RoleID).Select(p => p.Name).FirstOrDefault();

                counter++;
            }
            //roleTable[counter] = "Admin";
            return roleTable;
        }

        public void SaveUserRole(int userID, int roleID)
        {
            context.UserRoleAssociates.Add(new UserRoleAssociate
            {
                UserID = userID,
                RoleID = roleID
            });
        }
    }
}