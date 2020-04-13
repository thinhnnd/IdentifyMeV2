using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Hyperledger.Aries.Agents.Edge;
using Hyperledger.Aries.Configuration;
using Microsoft.Extensions.Options;
using IdentifyMe.Configuration;
using IdentifyMe.Framework.Services;
using IdentifyMe.MVVM;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Agents;
using System.Collections.Generic;
using Autofac;
using System.Linq;
using IdentifyMe.Events;
using Hyperledger.Aries.Contracts;
using System.Reactive.Linq;

namespace IdentifyMe.ViewModels.Connections
{
    public class ConnectionsViewModel : BaseNavigationViewModel, INavigationAware
    {
        private readonly IEdgeProvisioningService _edgeProvisioningService;
        private readonly IWalletAppConfiguration _walletConfiguration;
        private readonly AgentOptions _options;
        private CloudWalletService _cloudWalletService;
        private readonly IAgentProvider _agentProvider;
        private readonly IConnectionService _connectionService;
        private readonly ILifetimeScope _scope;
        private readonly IEventAggregator _eventAggregator;
        public ICommand CreateWalletCommand { get; }
        public ICommand GoToScanCommand { get; }

        public ICommand FetchInboxCommand { get; }

        public ICommand RefreshingCommand { get;  }

        public ConnectionsViewModel(IEdgeProvisioningService edgeProvisioningService, 
            IWalletAppConfiguration walletconfiguration, 
            IOptions<AgentOptions> options, 
            IAgentProvider agentProvider,
            IConnectionService connectionService,
            ILifetimeScope scope,
            IEventAggregator eventAggregator,
            CloudWalletService cloudWalletService)
        {
            Title = "Connections";
            _edgeProvisioningService = edgeProvisioningService;
            _walletConfiguration = walletconfiguration;
            _options = options.Value;
            _cloudWalletService = cloudWalletService;
            _connectionService = connectionService;
            _agentProvider = agentProvider;
            _eventAggregator = eventAggregator;
            _scope = scope;

            CreateWalletCommand = new AsyncCommand(CreateAgent);
            GoToScanCommand = new AsyncCommand(GoToScan);
            FetchInboxCommand = new AsyncCommand(FetchInbox);
            RefreshingCommand = new AsyncCommand(RefreshConnectionsList);
            Task.Run(async () => await RefreshConnectionsList());
        }

        public override async Task InitAsync()
        {
            await base.InitAsync();
            Console.WriteLine("Init Async work");

            await RefreshConnectionsList();
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();
            Console.WriteLine("On Apperring works");
            try
            {
                await RefreshConnectionsList();

                _eventAggregator.GetEventByType<ApplicationEvent>()
                   .Where(_ => _.Type == ApplicationEventType.ConnectionsUpdated)
                   .Subscribe(async _ => await RefreshConnectionsList());
            } 
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        public void OnNavigatedTo()
        {
            Console.WriteLine(@"Just got here");
        }

        public void OnNavigatingFrom()
        {
            Console.WriteLine(@"Navigating Out");
        }

        public void OnNavigatingTo()
        {
            Console.WriteLine(@"Navigating Out");
        }

        private async Task CreateAgent()
        {
            Preferences.Set(Constants.PoolConfigurationName, _walletConfiguration.PoolConfigurationName);
            IsBusy = true;
            try
            {
                _options.AgentName = DeviceInfo.Name;
                _options.WalletConfiguration.Id = Constants.LocalWalletIdKey;
                _options.WalletCredentials.Key = await SyncedSecureStorage.GetOrCreateSecureAsync(
                    key: Constants.LocalWalletCredentialKey,
                    value: Utils.Utils.GenerateRandomAsync(32));
                await _edgeProvisioningService.ProvisionAsync(_options);
                await Application.Current.MainPage.DisplayAlert("Wallet created", "", "Ok");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, "Wasn't able to provision the agent");
                await Application.Current.MainPage.DisplayAlert("An error occured while registering the wallet", "", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
        public async Task RefreshConnectionsList()
        {
            RefreshingConnections = true;
            var context = await _agentProvider.GetContextAsync();
            if (context != null)
            {
                try
                {
                    var records = await _connectionService.ListAsync(context);

                    IList<ConnectionViewModel> connectionViewModels = new List<ConnectionViewModel>();

                    foreach (var record in records)
                    {
                        if (record.Alias != null)
                        {
                            var connection = _scope.Resolve<ConnectionViewModel>(new NamedParameter("record", record));
                            connectionViewModels.Add(connection);
                        }

                    }

                    Connections.Clear();

                    foreach (var connectionVm in connectionViewModels)
                    {
                        Connections.Add(connectionVm);
                    }
                    //Connections = connectionViewModels;
                    HasConnections = connectionViewModels.Any();
                    RefreshingConnections = false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    RefreshingConnections = false;

                }
            }
            RefreshingConnections = false;
            await Application.Current.MainPage.DisplayAlert("Can't load connections", "", "Ok");

        }


        private async Task GoToScan()
        {
            await Navigation.PushAsync(MakeVm<ScanCodeViewModel>());
        }

        private async Task FetchInbox()
        {
            await _cloudWalletService.FetchCloudMessagesAsync();
        }

        private bool _refreshingConnections;

        public bool RefreshingConnections
        {
            get => _refreshingConnections;
            set => RaiseAndUpdate(ref _refreshingConnections, value);
        }

        private bool _hasConnections;
        public bool HasConnections
        {
            get => _hasConnections;
            set => this.RaiseAndUpdate(ref _hasConnections, value);
        }

        private ObservableCollection<ConnectionViewModel> _connections = new ObservableCollection<ConnectionViewModel>();

        public ObservableCollection<ConnectionViewModel> Connections
        {
            get => _connections;
            set => RaiseAndUpdate<ObservableCollection<ConnectionViewModel>>(ref _connections, value, "string");
        }
    }
}
