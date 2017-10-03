using System.Collections.Specialized;

namespace LuzzedroCMS.Infrastructure.Abstract
{
    public interface IConfigurationManager
    {
        NameValueCollection AppSettings { get; }
    }
}
