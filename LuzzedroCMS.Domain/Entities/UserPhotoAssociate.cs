using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LuzzedroCMS.Domain.Entities
{
    [Table("LCMS_UserPhotoAssociate")]
    public class UserPhotoAssociate
    {
        public int UserPhotoAssociateID { get; set; }
        public int UserID { get; set; }
        public int PhotoID { get; set; }
        public DateTime Date { get; set; }
        public int Status { get; set; }
    }
}