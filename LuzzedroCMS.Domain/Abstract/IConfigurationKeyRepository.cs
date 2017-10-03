using LuzzedroCMS.Domain.Entities;
using System.Linq;

namespace LuzzedroCMS.Domain.Abstract
{
    public interface IConfigurationKeyRepository
    {
        string Get(string key);
        ConfigurationKey Key(string key);
        IQueryable<ConfigurationKey> ConfigurationKeys { get; }
        int Save(ConfigurationKey configurationKey);
    }
}