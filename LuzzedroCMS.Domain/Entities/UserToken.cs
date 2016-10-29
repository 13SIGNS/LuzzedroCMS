using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LuzzedroCMS.Domain.Entities
{
    [Table("LCMS_UserToken")]
    public class UserToken
    {
        public int UserTokenID { get; set; }
        public int UserID { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}