using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LuzzedroCMS.Domain.Entities
{
    [Table("LCMS_UserRoleAssociate")]
    public class UserRoleAssociate
    {
        public int UserRoleAssociateID { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }
    }
}