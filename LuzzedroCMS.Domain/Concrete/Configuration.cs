using LuzzedroCMS.Domain.Entities;
using System.Collections.Generic;
using System.Data.Entity.Migrations;

namespace LuzzedroCMS.Domain.Concrete
{
    public class Configuration : DbMigrationsConfiguration<EFDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(EFDbContext context)
        {
            if (context.ConfigurationKeys == null)
            {
                var keys = new List<ConfigurationKey>
                {
                    new ConfigurationKey {Key="ApplicationName",Value="LuzzedroCMS"},
                    new ConfigurationKey {Key="MainTitle", Value="LuzzedroCMS"},
                    new ConfigurationKey {Key="MainKeywords", Value="LuzzedroCMS"},
                    new ConfigurationKey {Key="MainDescription", Value="LuzzedroCMS"},
                    new ConfigurationKey {Key="UseFtpForExternalContent", Value="true"},
                    new ConfigurationKey {Key="FtpCredentialHost", Value="ftp://" },
                    new ConfigurationKey {Key="FtpCredentialUser", Value="luzzedro"},
                    new ConfigurationKey {Key="FtpCredentialPassword", Value="passwd"},
                    new ConfigurationKey {Key="FtpPath", Value="/mydata/" },
                    new ConfigurationKey {Key="Url", Value="http://mywebsite.xyz" },
                    new ConfigurationKey {Key="ContentExternalUrl", Value="http://mywebsite.lol/mydata/" },
                    new ConfigurationKey {Key="FacebookAppId", Value="" },
                    new ConfigurationKey {Key="FacebookAppSecret", Value="" },
                    new ConfigurationKey {Key="GoogleAppId", Value="" },
                    new ConfigurationKey {Key="GoogleAppSecret", Value="" },
                    new ConfigurationKey {Key="GoogleAppId", Value="" },
                    new ConfigurationKey {Key="Regulations", Value="<h1>Regulations</h1>" },
                    new ConfigurationKey {Key="IsFacebookConnected", Value="false" },
                    new ConfigurationKey {Key="FacebookAppLink", Value="" },
                    new ConfigurationKey {Key="IsGoogleConnected", Value="false" },
                    new ConfigurationKey {Key="IsGoogleAnalyticsConnected", Value="false" },
                    new ConfigurationKey {Key="GoogleAnalyticsId", Value="" },
                    new ConfigurationKey {Key="About", Value="" }
                };

                keys.ForEach(s => context.ConfigurationKeys.Add(s));
                context.SaveChanges();
            }

            if (context.Roles == null)
            {
                var roles = new List<Role>
                {
                    new Role {RoleID=1 , Name="Admin"},
                    new Role {RoleID=2 , Name="User"}
                };

                roles.ForEach(s => context.Roles.Add(s));
                context.SaveChanges();
            }
        }
    }
}