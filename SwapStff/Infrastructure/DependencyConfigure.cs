using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using Autofac;
//using Autofac.Builder;
//using Autofac.Core;
//using Autofac.Integration.Mvc;
using System.Web.Mvc;
using SwapStff.Data;
using SwapStff.Entity;
using SwapStff.Service;
using SwapStff.Core.Cache;
using System.Web.Security;
using System.Configuration;
//using SwapStff.Web.Infrastructure.Providers;

namespace SwapStff.Infrastructure
{
    internal class DependencyConfigure
    {
        public static void Initialize()
        {
            var builder = new ContainerBuilder();
            DependencyResolver.SetResolver(
               new AutofacDependencyResolver(RegisterServices(builder))
                );
        }
        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            var cs = ConfigurationManager.ConnectionStrings["DataContext"].ConnectionString;

            builder.RegisterAssemblyTypes(
                typeof(WebApiApplication).Assembly
                ).PropertiesAutowired();

            
            //deal with your dependencies here
            builder.Register(c => new DataContext(cs)).As<IDbContext>().InstancePerHttpRequest();
            //builder.RegisterType<DataContext>().As<IDbContext>().InstancePerHttpRequest();

            builder.RegisterGeneric(typeof(RepositoryService<>)).As(typeof(IRepository<>)).InstancePerHttpRequest();

            builder.RegisterType<ProfileService>().As<IProfileService>().InstancePerHttpRequest();
            builder.RegisterType<ItemService>().As<IItemService>().InstancePerHttpRequest();
            builder.RegisterType<ItemMatchService>().As<IItemMatchService>().InstancePerHttpRequest();
            builder.RegisterType<ChatService>().As<IChatService>().InstancePerHttpRequest();

            builder.RegisterType<LoggerService>().As<ILoggerService>().InstancePerHttpRequest();
            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().Named<ICacheManager>("Profile_contest_cache_static").InstancePerHttpRequest();

            //builder.RegisterType<AccountMembershipProvider>().InstancePerHttpRequest();
            //builder.Register(c => c.Resolve<AccountMembershipProvider>()).As<MembershipProvider>().InstancePerHttpRequest();


            return
                builder.Build();
        }
       
    }
}