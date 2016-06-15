using Domain;
using Domain.Abstract;
using Domain.Concrete;
using Moq;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
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
            //Mock<IThemeRepository> mock = new Mock<IThemeRepository>();
            //mock.Setup(m => m.Themes).Returns(new List<Theme>
            //{
            //    new Theme {Id = 1, Title = ".Net", Description = "Best thing for beginners", TeacherId = 1},
            //    new Theme {Id = 2, Title = "Asp.Net", Description = "That`s harder", TeacherId = 2},
            //    new Theme {Id = 3, Title = "C#", Description = "You know it", TeacherId = 3},
                
            //});
            //kernel.Bind<IThemeRepository>().ToConstant(mock.Object);
            kernel.Bind<IThemeRepository>().To<EFThemeRepository>();
            kernel.Bind<IArticleRepository>().To<EFArticleRepository>();
            kernel.Bind<ICommentRepository>().To<EFCommentRepository>();
        }
    }
}