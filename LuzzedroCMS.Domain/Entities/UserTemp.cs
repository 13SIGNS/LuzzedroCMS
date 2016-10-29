using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LuzzedroCMS.Domain.Entities
{
    [Table("LCMS_UserTemp")]
    public class UserTemp
    {
        public int UserTempID { get; set; }
        public DateTime Date { get; set; }
        public string Email { get; set; }
        public string Nick { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}