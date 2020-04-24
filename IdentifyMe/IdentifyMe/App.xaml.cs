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
                    Container.Resolve<INavigationService>().RegisterViewModels(typeof(BaseViewModel).Assembly);
                    _navigationService = Container.Resolve<INavigationServiceV2>();

                });
        static Task InitializeTask;
        static INavigationServiceV2 _navigationService;
        private async static Task Initialize()
        {
            _navigationService.AddPageViewModelBinding<TestViewModel, TestPage>();
            _navigationService.AddPageViewModelBinding<ConnectionsViewModelV2, ConnectionsPageV2>();
            _navigationService.AddPageViewModelBinding<NotificationViewModelV2, NotificationV2>();
            _navigationService.AddPageViewModelBinding<CredentialsViewModelV2, CredentialsPageV2>();
            _navigationService.AddPageViewModelBinding<CredOfferViewModelV2, CredOfferPageV2>();
            _navigationService.AddPageViewModelBinding<ProofRequestViewModelV2, ProofRequestPageV2>();
            _navigationService.AddPageViewModelBinding<ScanCodeViewModelV2, ScanCodePageV2>();
            _navigationService.AddPopupViewModelBinding<AcceptInvitationViewModelV2, AcceptInvitationPopupV2>();
            _navigationService.AddPageViewModelBinding<MainViewModel, MainPageV2>();
            await _navigationService.NavigateToAsync<MainViewModel>();
            //if (_contextProvider.AgentExists())
            //{
            //    await _navigationService.NavigateToAsync<MainViewModel>();
            //}
            //else
            //{
            //    await _navigationService.NavigateToAsync<RegisterViewModel>();
            //}
        }
        protected override void OnStart()
        {
            Host.Start();
            InitializeTask = Initialize();

            //if (Preferences.Get("LocalWalletProvisioned", false))
            //{
            //    var mainPage = Container.Resolve<MainPage>();
            //    mainPage.ViewModel = Container.Resolve<MainPageViewModel>();
            //    MainPage = new NavigationPage(mainPage);
            //    var message = new StartLongRunningTaskMessage();
            //    MessagingCenter.Send(message, "StartLongRunningTaskMessage");
            //    HandleReceivedMessages();
            //}
            //else
            //{
            //    var registerPage = Container.Resolve<RegisterPage>();
            //    registerPage.ViewModel = Container.Resolve<RegisterPageViewModel>();
            //    MainPage = new NavigationPage(registerPage);
            //}
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

        void HandleReceivedMessages()
        {
            System.Diagnostics.Debug.WriteLine($"Tikcked Messsage 1: work");
            MessagingCenter.Subscribe<TickedMessage>(this, "TickedMessage", message => {

                Device.BeginInvokeOnMainThread(async () => {
                    System.Diagnostics.Debug.WriteLine($"Tikcked Messsage 2: { message.Message}");
                    _cloudWalletService = Container.Resolve<CloudWalletService>();
                    await _cloudWalletService.FetchCloudMessagesAsync();
                    
                    //_cloudWalletService = Container.Resolve<CloudWalletService>();
                   // await _cloudWalletService.FetchCloudMessagesAsync();
                });
            });

            MessagingCenter.Subscribe<CancelledMessage>(this, "CancelledMessage", message => {
                Device.BeginInvokeOnMainThread(() => {
                    //ticker.Text = "Cancelled";
                });
            });
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
