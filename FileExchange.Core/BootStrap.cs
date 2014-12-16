using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileExchange.Core.Repositories;
using FileExchange.Core.Services;

namespace FileExchange.Core
{
    /// <summary>
    /// temporary class
    /// </summary>
    public static class BootStrap
    {
        public static IContainer Container { get; private set; }

        static BootStrap()
        {
            var builder = new ContainerBuilder();
            builder.RegisterGeneric(typeof (GenericRepository<>)).As(typeof(IGenericRepository<>));
            Container = builder.Build();
        }
    }
}
