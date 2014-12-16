using Autofac;
using FileExchange.Core.Services;
using FileExchange.Core.UOW;

namespace FileExchange.Core.AutofacModules
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<ExchangeFileService>().As<IExchangeFileService>()
                .UsingConstructor(typeof(IUnitOfWork))
                .InstancePerRequest();
            base.Load(builder);
        }
    }
}