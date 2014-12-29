using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using FileExchange.Infrastructure.AutofacModules;

namespace FileExchange
{
    public class AutofacConfig
    {
        public static class ApplicationContainer
        {
            public static IContainer Container { get; set; }
        }
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterFilterProvider();

            builder.RegisterSource(new ViewRegistrationSource());

            builder.RegisterModule(new GeneralModule());

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
          
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            ApplicationContainer.Container = container;
        } 
    }
}