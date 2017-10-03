using LuzzedroCMS.Domain.Abstract;
using LuzzedroCMS.Domain.Entities;
using System;
using System.Linq;

namespace LuzzedroCMS.Domain.Concrete
{
    public class EFConfigurationKeyRepository : IConfigurationKeyRepository
    {
        private EFDbContext context = new EFDbContext();

        public string Get(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                return this.Key(key) != null ? this.Key(key).Value : null;
            }
            else
            {
                return null;
            }
        }

        public ConfigurationKey Key(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                return context.ConfigurationKeys.FirstOrDefault(x => x.Key == key);
            }
            else
            {
                return null;
            }
        }

        public IQueryable<ConfigurationKey> ConfigurationKeys
        {
            get
            {
                return context.ConfigurationKeys;
            }
        }

        public int Save(ConfigurationKey configurationKey)
        {
            ConfigurationKey dbEntry;
            if (configurationKey != null && configurationKey.Key != null)
            {
                dbEntry = context.ConfigurationKeys.FirstOrDefault(x => x.Key == configurationKey.Key);
                if (dbEntry != null)
                {
                    dbEntry.Key = configurationKey.Key;
                    dbEntry.Value = configurationKey.Value;
                }
                context.SaveChanges();
                return dbEntry.ConfigurationKeyID;
            }
            else
            {
                return 0;
            }
        }
    }
}