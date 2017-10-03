using LuzzedroCMS.Domain.Entities;
using System.Linq;

namespace LuzzedroCMS.Models
{
    public class ConfigurationViewModel
    {
        public IQueryable<ConfigurationKey> ConfigurationKeys { get; set; }
    }
}