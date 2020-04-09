using Autofac;
using Hyperledger.Aries.Agents;
using IdentifyMe.Framework.Services;
using IdentifyMe.MVVM;

namespace IdentifyMe.DependencyInjection
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NavigationService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<MobileAgentProvider>()
                .AsImplementedInterfaces()
                .SingleInstance();
        
            builder.RegisterType<CloudWalletService>()
                .AsSelf()
                .SingleInstance();
            
            builder.RegisterAssemblyTypes(typeof(ServicesModule).Assembly)
                .Where(x => typeof(IAgentMiddleware).IsAssignableFrom(x))
                .AsImplementedInterfaces()
                .SingleInstance();

        }
    }
}
