using LuzzedroCMS.Domain.Entities;
using System.Web;

namespace LuzzedroCMS.Models
{
    public class CategoryViewModel : Category
    {
        public string CategoryNameEscaped
        {
            get
            {
                return base.Name.Replace(" ", "-");
            }
        }
    }
}