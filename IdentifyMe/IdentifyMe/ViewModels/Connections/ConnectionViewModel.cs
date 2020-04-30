using Acr.UserDialogs;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Contracts;
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Features.Discovery;
using IdentifyMe.Extensions;
using IdentifyMe.Services.Interfaces;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace IdentifyMe.ViewModels.Connections
{
    public class ConnectionViewModel : ABaseViewModel
    {
        private readonly IAgentProvider _agentProvider;
        private readonly IMessageService _messageService;
        private readonly IDiscoveryService _discoveryService;
        private readonly IConnectionService _connectionService;
        private readonly IEventAggregator _eventAggregator;
        private readonly ConnectionRecord _record;

        public ConnectionViewModel(IUserDialogs userDialogs,
            INavigationService navigationService,
            IAgentProvider agentProvider,
            IMessageService messageService,
            IDiscoveryService discoveryService,
            IConnectionService connectionService,
            IEventAggregator eventAggregator,
            ConnectionRecord record) : base (nameof(ConnectionsViewModel), userDialogs, navigationService)
        {
            _agentProvider = agentProvider;
            _messageService = messageService;
            _discoveryService = discoveryService;
            _connectionService = connectionService;
            _eventAggregator = eventAggregator;
            _record = record;

            MyDid = _record.MyDid;
            TheirDid = _record.TheirDid;
            ConnectionName = _record.Alias.Name;
            ConnectionSubtitle = $"{_record.State:G}";
            ConnectionImageUrl = _record.Alias.ImageUrl;
        }

        #region Bindable Properties
        private string _connectionName;
        public string ConnectionName
        {
            get => _connectionName;
            set => this.RaiseAndSetIfChanged(ref _connectionName, value);
        }

        private string _myDid;
        public string MyDid
        {
            get => _myDid;
            set => this.RaiseAndSetIfChanged(ref _myDid, value);
        }

        private string _theirDid;
        public string TheirDid
        {
            get => _theirDid;
            set => this.RaiseAndSetIfChanged(ref _theirDid, value);
        }

        private string _connectionImageUrl;
        public string ConnectionImageUrl
        {
            get => _connectionImageUrl;
            set => this.RaiseAndSetIfChanged(ref _connectionImageUrl, value);
        }

        private string _connectionSubtitle = "Lorem ipsum dolor sit amet";
        public string ConnectionSubtitle
        {
            get => _connectionSubtitle;
            set => this.RaiseAndSetIfChanged(ref _connectionSubtitle, value);
        }

        private RangeEnabledObservableCollection<TransactionItem> _transactions = new RangeEnabledObservableCollection<TransactionItem>();
        public RangeEnabledObservableCollection<TransactionItem> Transactions
        {
            get => _transactions;
            set => this.RaiseAndSetIfChanged(ref _transactions, value);
        }

        private bool _refreshingTransactions;
        public bool RefreshingTransactions
        {
            get => _refreshingTransactions;
            set => this.RaiseAndSetIfChanged(ref _refreshingTransactions, value);
        }

        private bool _hasTransactions;
        public bool HasTransactions
        {
            get => _hasTransactions;
            set => this.RaiseAndSetIfChanged(ref _hasTransactions, value);
        }

        #endregion
    }
}
