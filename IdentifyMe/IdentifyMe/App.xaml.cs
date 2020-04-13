using System;
using System.Diagnostics;
using Xamarin.Forms;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Autofac;
using System.Reflection;
using IdentifyMe.DependencyInjection;
using Autofac.Extensions.DependencyInjection;
//using Microsoft.AppCenter.Push;
using IdentifyMe.Framework.Services;
using IdentifyMe.MVVM;
using IdentifyMe.MVVM.Abstractions;
using Xamarin.Essentials;
using IdentifyMe.Services;
using Hyperledger.Aries.Storage;
using System.IO;
using IdentifyMe.Configuration;
using IdentifyMe.Views;
using IdentifyMe.ViewModels;
using Acr.UserDialogs;
using Hyperledger.Aries.Agents;
using System.Threading.Tasks;

namespace IdentifyMe
{
    public partial class App : Application
    {
        public static IContainer Container { get; set; }
        public App() => InitializeComponent();

        public App(IHost host) : this() => Host = host;

        public static IHost Host { get; private set; }

        public static IHostBuilder BuildHost(Assembly platformSpecific = null) =>
            XamarinHost.CreateDefaultBuilder<App>()
                .ConfigureServices((context, services) =>
                {
                    services.AddAriesFramework();
                    services.AddAriesFramework(builder => builder.RegisterEdgeAgent(
                    options: options =>
                    {
                        var config = Container.Resolve<IWalletAppConfiguration>();
                        options.EndpointUri = config.AgentFrameworkEndpoint;

                        options.WalletConfiguration.StorageConfiguration = new WalletConfiguration.WalletStorageConfiguration
                        {
                            Path = Path.Combine(
                                path1: FileSystem.AppDataDirectory,
                                path2: ".indy_client",
                                path3: "wallets")
                        };
                        options.WalletConfiguration.Id = Constants.LocalWalletIdKey;
                    },
                    delayProvisioning: true));

                    services.AddHostedService<PoolConfigurator>();
                    services.OverrideDefaultAgentProvider<MobileAgentProvider>();

                    var containerBuilder = new ContainerBuilder();
                    containerBuilder.RegisterAssemblyModules(typeof(ViewModelsModule).Assembly);
                    if (platformSpecific != null)
                    {
                        containerBuilder.RegisterAssemblyModules(platformSpecific);
                    }
                    containerBuilder.Populate(services);

                    Container = containerBuilder.Build();
                    
                    Container.Resolve<INavigationService>().RegisterViewModels(typeof(BaseViewModel).Assembly);

                });

        protected override void OnStart()
        {
            Host.Start();

            if (Preferences.Get("LocalWalletProvisioned", false))
            {
                var mainPage = Container.Resolve<MainPage>();
                mainPage.ViewModel = Container.Resolve<MainPageViewModel>();
                MainPage = new NavigationPage(mainPage);
            }
            else
            {
                var registerPage = Container.Resolve<RegisterPage>();
                registerPage.ViewModel = Container.Resolve<RegisterPageViewModel>();
                MainPage = new NavigationPage(registerPage);
            }
        }

        protected override void OnAppLinkRequestReceived(Uri uri)
        {
            // Deeplink functionality code here
        }

        protected override void OnSleep()
        {
            Host.Sleep();
        }

        protected override void OnResume()
        {
            Host.Resume();
        }

        //public async void HandlePushNotification(PushNotificationReceivedEventArgs e)
        //{
        //    Debug.WriteLine($"Push Message: {e.Title} - {e.Message}");

        //    try
        //    {
        //        var count = await Container.Resolve<CloudWalletService>().FetchCloudMessagesAsync();
        //        Debug.WriteLine($"Processed {count} cloud messages");
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex);
        //    }
        //}

        public void OpenUrl(Uri uri)
        {
            OnAppLinkRequestReceived(uri);
        }
    }
}
