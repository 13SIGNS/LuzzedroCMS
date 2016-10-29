using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LuzzedroCMS.Domain.Entities
{
    [Table("LCMS_Log")]
    public class Log
    {
        public int LogID { get; set; }
        public int UserID { get; set; }
        public DateTime Date { get; set; }
    }
}