using Autofac;

namespace IdentifyMe.DependencyInjection
{
    public class ViewModelsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(ThisAssembly)
                .Where(x => x.Namespace.Contains("IdentifyMe.ViewModels"))
                .InstancePerDependency();
        }
    }
}
