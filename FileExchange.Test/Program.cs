using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FileExchange.Core;
using FileExchange.Core.AutofacModules;
using FileExchange.Core.BusinessObjects;
using FileExchange.Core.Repositories;
using FileExchange.Core.Services;
using FileExchange.Core.UOW;

namespace FileExchange.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var builder = new ContainerBuilder();
                builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

                builder.RegisterType<ExchangeFileService>().As<IExchangeFileService>()
                    .UsingConstructor(typeof (IUnitOfWork));
                builder.RegisterType<FileCategoriesService>().As<IFileCategoriesService>()
                    .UsingConstructor(typeof (IUnitOfWork));
                builder.RegisterType<FileCommentService>().As<IFileCommentService>()
                    .UsingConstructor(typeof (IUnitOfWork));
                builder.RegisterType<FileNotificationSubscriberService>().As<IFileNotificationSubscriberService>()
                    .UsingConstructor(typeof (IUnitOfWork));
                builder.RegisterType<NewsService>().As<INewsService>()
                    .UsingConstructor(typeof (IUnitOfWork));
                var container = builder.Build();

                
                var unitOfWork = container.Resolve<IUnitOfWork>();
                var exhcangeFileService = container.Resolve<IExchangeFileService>();
                exhcangeFileService.GetAll();
                var tmp = unitOfWork;
            }
            catch (ModelValidationException exc)
            {
                foreach (var excMes in exc.Data)
                {
                    Console.Write(excMes);
                }
            }
            Console.ReadKey();
        }
    }
}
