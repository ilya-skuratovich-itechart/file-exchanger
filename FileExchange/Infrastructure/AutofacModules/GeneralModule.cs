using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using FileExchange.Controllers;
using FileExchange.Core.BandwidthThrottling;
using FileExchange.Core.Services;
using FileExchange.Core.UOW;
using FileExchange.EmailSender;
using FileExchange.Infrastructure.Captcha;
using FileExchange.Infrastructure.FileHelpers;
using FileExchange.Infrastructure.Filtres;
using FileExchange.Infrastructure.LoggerManager;
using FileExchange.Infrastructure.UserSecurity;
using FileExchange.Infrastructure.ViewsHelpers;
using Module = Autofac.Module;

namespace FileExchange.Infrastructure.AutofacModules
{
    public class GeneralModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterFilters(builder);

            builder.Register(c => new
                Mailer("fileExchange@localhost", "test", "localhost", 25)).As<IMailer>();

            builder.Register(c => new WebSecurityWrapper()).As<IWebSecurity>();
            
            builder.Register(c => new FileExchange.Infrastructure.Captcha.Captcha()).As<ICaptcha>();

            builder.Register(c => new
                FileProvider()).As<IFileProvider>().SingleInstance();

            builder.Register(c => new
                LoggerManager.LoggerManager()).As<ILogger>().SingleInstance();

            builder.Register(c => new
             ViewRenderWrapper()).As<IViewRenderWrapper>();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerHttpRequest();

            builder.RegisterType<ExchangeFileService>().As<IExchangeFileService>()
                .UsingConstructor(typeof(IUnitOfWork))
                .InstancePerHttpRequest();


            builder.RegisterType<UserRolesService>().As<IUserRolesService>()
                .UsingConstructor(typeof(IUnitOfWork))
                .InstancePerHttpRequest();

            builder.RegisterType<UserInRolesService>().As<IUserInRolesService>()
                .UsingConstructor(typeof(IUnitOfWork))
                .InstancePerHttpRequest();


            builder.RegisterType<UserProfileService>().As<IUserProfileService>()
                .UsingConstructor(typeof(IUnitOfWork)).InstancePerHttpRequest();

            builder.RegisterType<FileCommentService>().As<IFileCommentService>()
                .UsingConstructor(typeof(IUnitOfWork))
                .InstancePerHttpRequest();

            builder.RegisterType<FileCategoriesService>().As<IFileCategoriesService>()
                .UsingConstructor(typeof(IUnitOfWork))
                .InstancePerHttpRequest();

            builder.RegisterType<FileNotificationSubscriberService>().As<IFileNotificationSubscriberService>()
                .UsingConstructor(typeof(IUnitOfWork))
                .InstancePerHttpRequest();

            builder.RegisterType<NewsService>().As<INewsService>()
                .UsingConstructor(typeof(IUnitOfWork))
                .InstancePerHttpRequest();


            builder.RegisterType<GlobalSettingService>().As<IGlobalSettingService>()
                .UsingConstructor(typeof(IUnitOfWork))
                .InstancePerHttpRequest();

            builder.RegisterType<BandwidthThrottlingSettings>().As<IBandwidthThrottlingSettings>()
                .UsingConstructor(typeof(IGlobalSettingService), typeof(IUserProfileService))
                .InstancePerHttpRequest();

            base.Load(builder);
        }

        private void RegisterFilters(ContainerBuilder builder)
        {
            Dictionary<string, List<string>> excludeBannedUserDic = new Dictionary<string, List<string>>()
            {
                {MVC.Account.Name, new List<string>() {MVC.Account.ActionNames.UserBanned}},
            };
      
            builder.Register(c => new BannedUserFilter(excludeBannedUserDic,c.Resolve<IUserProfileService>()))
                .AsAuthorizationFilterFor<HomeController>().InstancePerHttpRequest();

            builder.Register(c => new BannedUserFilter(excludeBannedUserDic, c.Resolve<IUserProfileService>()))
               .AsAuthorizationFilterFor<AccountController>().InstancePerHttpRequest();

            builder.Register(c => new BannedUserFilter(excludeBannedUserDic, c.Resolve<IUserProfileService>()))
               .AsAuthorizationFilterFor<FileController>().InstancePerHttpRequest();

            builder.Register(c => new BannedUserFilter(excludeBannedUserDic, c.Resolve<IUserProfileService>()))
               .AsAuthorizationFilterFor<NewsController>().InstancePerHttpRequest();
        }
    }
}