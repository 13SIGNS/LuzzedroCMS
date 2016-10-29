using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LuzzedroCMS.Domain.Entities
{
    [Table("LCMS_Role")]
    public class Role
    {
        public int RoleID { get; set; }
        public string Name { get; set; }
    }
}