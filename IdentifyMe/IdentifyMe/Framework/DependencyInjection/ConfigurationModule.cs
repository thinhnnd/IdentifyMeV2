using System.Reflection;
using Autofac;
using IdentifyMe.Configuration;
using IdentifyMe.Utils;
using Module = Autofac.Module;

namespace IdentifyMe.Framework.DependencyInjection
{
    public class ConfigurationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(
                    c => Assembly.GetAssembly(typeof(IWalletAppConfiguration)).GetEmbeddedConfiguration())
                .SingleInstance();
        }
    }
}
