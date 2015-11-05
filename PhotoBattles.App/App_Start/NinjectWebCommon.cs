[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(PhotoBattles.App.App_Start.NinjectWebCommon), "Start")]
[assembly:
    WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(PhotoBattles.App.App_Start.NinjectWebCommon), "Stop")]

namespace PhotoBattles.App.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    using PhotoBattles.App.Contracts;
    using PhotoBattles.App.Infrastructure;
    using PhotoBattles.Data;
    using PhotoBattles.Data.Contracts;
    using PhotoBattles.Models;
    using PhotoBattles.Models.Contracts;
    using PhotoBattles.Models.Strategies.VotingStrategies;

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
        /// Creates the kernel that will manage
        /// 
        ///  your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
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
            kernel.Bind<IPhotoBattlesData>()
                  .To<PhotoBattlesData>()
                  .WithConstructorArgument("data", c => new PhotoBattlesContext());

            kernel.Bind<IPhotoBattlesContext>()
                  .To<PhotoBattlesContext>();

            kernel.Bind<IUserIdProvider>()
                  .To<AspNetUserIdProvider>();

            kernel.Bind<IContest>().To<Contest>();
        }
    }
}