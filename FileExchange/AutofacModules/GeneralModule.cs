using Autofac;
using Autofac.Integration.Mvc;
using FileExchange.Core.Services;
using FileExchange.Core.UOW;

namespace FileExchange.AutofacModules
{
    public class GeneralModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerHttpRequest();
            builder.RegisterType<FileCommentService>().As<IFileCommentService>()
                .UsingConstructor(typeof (IUnitOfWork))
                .InstancePerHttpRequest();

            builder.RegisterType<FileCategoriesService>().As<IFileCategoriesService>()
                .UsingConstructor(typeof (IUnitOfWork))
                .InstancePerHttpRequest();

            builder.RegisterType<ExchangeFileService>().As<IExchangeFileService>()
                .UsingConstructor(typeof (IUnitOfWork))
                .InstancePerHttpRequest();

            builder.RegisterType<FileNotificationSubscriberService>().As<IFileNotificationSubscriberService>()
                .UsingConstructor(typeof (IUnitOfWork))
                .InstancePerHttpRequest();

            builder.RegisterType<NewsService>().As<INewsService>()
                .UsingConstructor(typeof (IUnitOfWork))
                .InstancePerHttpRequest();

            base.Load(builder);
        }
    }
}