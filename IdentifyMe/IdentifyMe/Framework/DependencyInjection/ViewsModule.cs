using Autofac;
using Xamarin.Forms;

namespace IdentifyMe.DependencyInjection
{
    public class ViewsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(GetType().Assembly)
                        .Where(x => x.IsAssignableTo<Page>())
                        .AsSelf();
            builder
                .RegisterAssemblyTypes(ThisAssembly)
                .Where(x => x.Namespace.Contains("IdentifyMe.Views"))
                .InstancePerDependency();
        }
    }
}
