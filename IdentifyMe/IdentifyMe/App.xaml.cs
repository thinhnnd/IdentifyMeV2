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
using IdentifyMe.Messages;
using Plugin.LocalNotification;
using IdentifyMe.Services.Middlewares;
using IdentifyMe.Services.Interfaces;
using IdentifyMe.ViewModels.Connections;
using IdentifyMe.Views.Connections;
using IdentifyMe.ViewModels.Notification;
using IdentifyMe.Views.Notification;
using IdentifyMe.ViewModels.Credentials;
using IdentifyMe.Views.Credentials;
using IdentifyMe.ViewModels.Setting;
using IdentifyMe.Views.Setting;

namespace IdentifyMe
{
    public partial class App : Application
    {
        public static IContainer Container { get; set; }
        public static IServiceProvider Provider { get; set; }
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
                        options.ProtocolVersion = 2;
                    },
                    delayProvisioning: true));
                    

                    services.AddHostedService<PoolConfigurator>();
                    services.OverrideDefaultAgentProvider<MobileAgentProvider>();
                    var containerBuilder = new ContainerBuilder();
                    containerBuilder.RegisterAssemblyModules(typeof(ViewModelsModule).Assembly);
                    containerBuilder.RegisterModule(new CoreModule());
                    if (platformSpecific != null)
                    {
                        containerBuilder.RegisterAssemblyModules(platformSpecific);
                    }
                    containerBuilder.Populate(services);
                    //Container.Resolve<TestMiddleWare>();
                    Container = containerBuilder.Build();

                    //Container.Resolve<MessageHandleMiddleWare>();
                    _navigationService = Container.Resolve<INavigationService>();

                });
        Task InitializeTask;
        static INavigationService _navigationService;
        private async Task Initialize()
        {
            _navigationService.AddPageViewModelBinding<ConnectionsViewModel, ConnectionsPage>();
            _navigationService.AddPageViewModelBinding<ConnectionViewModel, ConnectionPage>();
            _navigationService.AddPageViewModelBinding<NotificationViewModel, NotificationPage>();
            _navigationService.AddPageViewModelBinding<CredentialsViewModel, CredentialsPage>();
            _navigationService.AddPageViewModelBinding<CredentialViewModel, CredentialPage>();
            _navigationService.AddPageViewModelBinding<CredOfferViewModel, CredOfferPage>();
            _navigationService.AddPageViewModelBinding<ProofRequestViewModel, ProofRequestPage>();
            _navigationService.AddPageViewModelBinding<ScanCodeViewModel, ScanCodePage>();
            _navigationService.AddPopupViewModelBinding<AcceptInvitationViewModel, AcceptInvitationPopup>();
            _navigationService.AddPageViewModelBinding<MainViewModel, MainPage>();
            _navigationService.AddPageViewModelBinding<RegisterPageViewModel, RegisterPage>();
            _navigationService.AddPageViewModelBinding<SettingViewModel, SettingPage>();
            if (Preferences.Get("LocalWalletProvisioned", false))
            {
                //Task.Run(async () => await _navigationService.NavigateToAsync<MainViewModel>());
                await _navigationService.NavigateToAsync<MainViewModel>();
            }
            else
            {
                //Task.Run(async () => await _navigationService.NavigateToAsync<RegisterPageViewModelV2>());
                //Task.Run(async () => await _navigationService.NavigateToAsync<RegisterPageViewModelV2>());
                await _navigationService.NavigateToAsync<RegisterPageViewModel>();
            }
        }
        protected override void OnStart()
        {
            Host.Start();
            InitializeTask = Initialize();

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

        public CloudWalletService _cloudWalletService;

        

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
