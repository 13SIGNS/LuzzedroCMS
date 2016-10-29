using LuzzedroCMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuzzedroCMS.Domain.Abstract
{
    public interface IRoleRepository
    {
        int RoleIdByName(string roleName);
        string[] RolesByUserEmail(string email);
        void SaveUserRole(int userID, int roleID);
    }
}