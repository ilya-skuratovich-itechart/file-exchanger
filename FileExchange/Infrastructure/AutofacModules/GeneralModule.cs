using System.IO;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using FileExchange.Core.BandwidthThrottling;
using FileExchange.Core.Services;
using FileExchange.Core.UOW;
using FileExchange.EmailSender;
using Module = Autofac.Module;

namespace FileExchange.Infrastructure.AutofacModules
{
    public class GeneralModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new
                Mailer("fileExchange@localhost", "test", "localhost", 25)).As<IMailer>();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerHttpRequest();

            builder.RegisterType<ExchangeFileService>().As<IExchangeFileService>()
                .UsingConstructor(typeof (IUnitOfWork))
                .InstancePerHttpRequest();


            builder.RegisterType<UserRolesService>().As<IUserRolesService>()
                .UsingConstructor(typeof (IUnitOfWork))
                .InstancePerHttpRequest();

            builder.RegisterType<UserInRolesService>().As<IUserInRolesService>()
                .UsingConstructor(typeof (IUnitOfWork))
                .InstancePerHttpRequest();

            builder.RegisterType<UserProfileService>().As<IUserProfileService>()
                .UsingConstructor(typeof (IUnitOfWork))
                .InstancePerHttpRequest();

            builder.RegisterType<FileCommentService>().As<IFileCommentService>()
                .UsingConstructor(typeof (IUnitOfWork))
                .InstancePerHttpRequest();

            builder.RegisterType<FileCategoriesService>().As<IFileCategoriesService>()
                .UsingConstructor(typeof (IUnitOfWork))
                .InstancePerHttpRequest();

            builder.RegisterType<FileNotificationSubscriberService>().As<IFileNotificationSubscriberService>()
                .UsingConstructor(typeof (IUnitOfWork))
                .InstancePerHttpRequest();

            builder.RegisterType<NewsService>().As<INewsService>()
                .UsingConstructor(typeof (IUnitOfWork))
                .InstancePerHttpRequest();


            builder.RegisterType<GlobalSettingService>().As<IGlobalSettingService>()
                .UsingConstructor(typeof (IUnitOfWork))
                .InstancePerHttpRequest();

            builder.RegisterType<BandwidthThrottlingSettings>().As<IBandwidthThrottlingSettings>()
                .UsingConstructor(typeof (IGlobalSettingService), typeof (IUserProfileService))
                .InstancePerHttpRequest();

            base.Load(builder);
        }
    }
}