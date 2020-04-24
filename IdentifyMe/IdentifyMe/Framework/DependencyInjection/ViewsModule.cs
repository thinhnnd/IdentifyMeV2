using Autofac;
using Xamarin.Forms;

namespace IdentifyMe.DependencyInjection
{
    public class ViewsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(ThisAssembly)
                .Where(x => x.Namespace.Contains("IdentifyMe.Views"))
                .InstancePerDependency();
        }
    }
}
