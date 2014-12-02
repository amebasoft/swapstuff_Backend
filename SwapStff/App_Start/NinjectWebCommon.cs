[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SwapStff.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(SwapStff.App_Start.NinjectWebCommon), "Stop")]

namespace SwapStff.App_Start
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using SwapStff.Data;
    using SwapStff.Entity;
    using SwapStff.Service;
    using SwapStff.Core.Cache;
    using System.Configuration;
    

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                
                System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new Ninject.WebApi.DependencyResolver.NinjectDependencyResolver(kernel);
                RegisterServices(kernel);
                DependencyResolver.SetResolver(new SwapStff.App_Start.NinjectMvcDependencyResolver(kernel));
                
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }
        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            var cs = ConfigurationManager.ConnectionStrings["DataContext"].ConnectionString;

            kernel.Bind<IDbContext>().To<DataContext>().InSingletonScope().WithConstructorArgument("connection", cs);
            kernel.Bind(typeof(IRepository<>)).To(typeof(RepositoryService<>));
            kernel.Bind<IErrorExceptionLogService>().To<ErrorExceptionLogService>();
            kernel.Bind<IAppSettingService>().To<AppSettingService>();
            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<IProfileService>().To<ProfileService>();
            kernel.Bind<IItemService>().To<ItemService>();
            kernel.Bind<IItemMatchService>().To<ItemMatchService>();
            kernel.Bind<IChatService>().To<ChatService>();
            kernel.Bind<ILoggerService>().To<LoggerService>();
            kernel.Bind<ICacheManager>().To<MemoryCacheManager>().Named("Profile_contest_cache_static");

        }        
    }
}
