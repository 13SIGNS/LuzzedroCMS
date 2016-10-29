using LuzzedroCMS.Abstract;
using LuzzedroCMS.Concrete;
using LuzzedroCMS.Domain.Abstract;
using LuzzedroCMS.Domain.Concrete;
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

            kernel.Bind<IEmailSender>().To<EmailSender>();
        }


    }
}