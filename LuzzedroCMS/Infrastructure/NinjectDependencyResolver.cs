using LuzzedroCMS.Domain.Abstract;
using LuzzedroCMS.Domain.Concrete;
using LuzzedroCMS.Domain.Infrastructure.Abstract;
using LuzzedroCMS.Domain.Infrastructure.Concrete;
using LuzzedroCMS.Infrastructure.Abstract;
using LuzzedroCMS.Infrastructure.Concrete;
using Ninject;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace LuzzedroCMS.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver()
        {
            kernel = new StandardKernel();
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IArticleRepository>().To<EFArticleRepository>();
            kernel.Bind<ICategoryRepository>().To<EFCategoryRepository>();
            kernel.Bind<ICommentRepository>().To<EFCommentRepository>();
            kernel.Bind<ILogRepository>().To<EFLogRepository>();
            kernel.Bind<IPhotoRepository>().To<EFPhotoRepository>();
            kernel.Bind<ITagRepository>().To<EFTagRepository>();
            kernel.Bind<IUserRepository>().To<EFUserRepository>();
            kernel.Bind<IConfigurationKeyRepository>().To<EFConfigurationKeyRepository>();

            kernel.Bind<IEmailSender>().To<EmailSender>();
            kernel.Bind<IConfigurationManager>().To<ConfigurationManager>();
            kernel.Bind<IImageModifier>().To<ImageModifier>();
            kernel.Bind<IAccount>().To<Account>()
                .WithConstructorArgument("emailSender", context => context.Kernel.Get<IEmailSender>())
                .WithConstructorArgument("configManager", context => context.Kernel.Get<IConfigurationKeyRepository>());
            kernel.Bind<IFtp>().To<Ftp>()
                .WithConstructorArgument("configManager", context => context.Kernel.Get<IConfigurationKeyRepository>());
            kernel.Bind<ISessionHelper>().To<SessionHelper>()
                .WithConstructorArgument("userRepo", context => context.Kernel.Get<IUserRepository>());
            kernel.Bind<ITextBuilder>().To<TextBuilder>();
            kernel.Bind<IImageHelper>().To<ImageHelper>()
                .WithConstructorArgument("configRepo", context => context.Kernel.Get<IConfigurationKeyRepository>())
                .WithConstructorArgument("imgModifier", context => context.Kernel.Get<IImageModifier>())
                .WithConstructorArgument("ft", context => context.Kernel.Get<IFtp>())
                .WithConstructorArgument("txtBuilder", context => context.Kernel.Get<ITextBuilder>());
        }
    }
}