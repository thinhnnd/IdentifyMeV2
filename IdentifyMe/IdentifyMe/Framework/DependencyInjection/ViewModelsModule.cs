using Autofac;
using IdentifyMe.MVVM;

namespace IdentifyMe.DependencyInjection
{
    public class ViewModelsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(BaseViewModel).Assembly)
                        .Where(x => x.IsAssignableTo<BaseViewModel>())
                        .AsSelf();

            builder
                .RegisterAssemblyTypes(ThisAssembly)
                .Where(x => x.Namespace.Contains("IdentifyMe.ViewModels"))
                .InstancePerDependency();
        }
    }
}
