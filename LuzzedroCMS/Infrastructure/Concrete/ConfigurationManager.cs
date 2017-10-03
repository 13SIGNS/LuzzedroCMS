using LuzzedroCMS.Infrastructure.Abstract;
using System.Collections.Specialized;

namespace LuzzedroCMS.Infrastructure.Concrete
{
    public class ConfigurationManager : IConfigurationManager
    {
        public NameValueCollection AppSettings
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings;
            }
        }
    }
}