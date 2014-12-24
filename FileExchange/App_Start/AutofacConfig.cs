using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
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

            builder.RegisterFilterProvider();

            builder.RegisterSource(new ViewRegistrationSource());

            builder.RegisterModule(new GeneralModule());

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            ApplicationContainer.Container = container;
        } 
    }
}