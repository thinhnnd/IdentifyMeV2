using Autofac;
using IdentifyMe.Droid.Services;

namespace IdentifyMe.Droid.DependencyInjection
{
    public class DroidServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ZXingHelper>().AsImplementedInterfaces().SingleInstance();
            base.Load(builder);
        }
    }
}

