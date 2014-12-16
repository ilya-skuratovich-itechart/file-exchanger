using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using FileExchange.AutofacModules;

namespace FileExchange
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterFilterProvider();

            builder.RegisterSource(new ViewRegistrationSource());

            builder.RegisterModule(new GeneralModule());

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        } 
    }
}