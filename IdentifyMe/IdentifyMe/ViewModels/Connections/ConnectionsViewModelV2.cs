using Acr.UserDialogs;
using Autofac;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Agents.Edge;
using Hyperledger.Aries.Configuration;
using Hyperledger.Aries.Contracts;
using Hyperledger.Aries.Features.DidExchange;
using IdentifyMe.Configuration;
using IdentifyMe.Framework.Services;
using IdentifyMe.Services.Interfaces;
using Microsoft.Extensions.Options;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IdentifyMe.ViewModels.Connections
{
    public class ConnectionsViewModelV2 : ABaseViewModel
    {
        private readonly IEdgeProvisioningService _edgeProvisioningService;
        private readonly IWalletAppConfiguration _walletConfiguration;
        private readonly AgentOptions _options;
        private CloudWalletService _cloudWalletService;
        private readonly IAgentProvider _agentProvider;
        private readonly IConnectionService _connectionService;
        private readonly ILifetimeScope _scope;
        private readonly IEventAggregator _eventAggregator;

        public ConnectionsViewModelV2(IUserDialogs userDialogs,
                                   INavigationServiceV2 navigationService,
                                   IConnectionService connectionService,
                                   IEdgeProvisioningService edgeProvisioningService,
                                   IWalletAppConfiguration walletconfiguration,
                                   IOptions<AgentOptions> options,
                                   IAgentProvider agentProvider,
                                   ILifetimeScope scope,
                                   IEventAggregator eventAggregator,
                                   CloudWalletService cloudWalletService ) :
                                   base("Connections", userDialogs, navigationService)
        {
            _edgeProvisioningService = edgeProvisioningService;
            _walletConfiguration = walletconfiguration;
            _options = options.Value;
            _cloudWalletService = cloudWalletService;
            _connectionService = connectionService;
            _agentProvider = agentProvider;
            _eventAggregator = eventAggregator;
            _scope = scope;           
        }

        public override async Task InitializeAsync(object navigationData)
        {
            await RefreshConnectionsList();
            await base.InitializeAsync(navigationData);
        }


        public async Task RefreshConnectionsList()
        {
            RefreshingConnections = true;
            var context = await _agentProvider.GetContextAsync();
            if (context != null)
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
                HasConnections = connectionViewModels.Any();
                RefreshingConnections = false;
            }

        }

        #region Bindable props

        private bool _refreshingConnections;

        public bool RefreshingConnections
        {
            get => _refreshingConnections;
            set => this.RaiseAndSetIfChanged(ref _refreshingConnections, value);
        }

        private bool _hasConnections;
        public bool HasConnections
        {
            get => _hasConnections;
            set => this.RaiseAndSetIfChanged(ref _hasConnections, value);
        }

        private ObservableCollection<ConnectionViewModel> _connections = new ObservableCollection<ConnectionViewModel>();

        public ObservableCollection<ConnectionViewModel> Connections
        {
            get => _connections;
            set => this.RaiseAndSetIfChanged(ref _connections, value);
        }

        #endregion

        #region Binable Command 
        public ICommand RefreshingCommand => new Command(async () => await RefreshConnectionsList());
        public ICommand GoToScanCommand => new Command(async () => await NavigationService.NavigateToAsync<ScanCodeViewModelV2>());
        #endregion
    }
}
