using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Contracts;
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Features.Discovery;
using IdentifyMe.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace IdentifyMe.ViewModels.Connections
{
    public class ConnectionViewModel : BaseNavigationViewModel, INavigationAware
    {
        private readonly IAgentProvider _agentProvider;
        private readonly IMessageService _messageService;
        private readonly IDiscoveryService _discoveryService;
        private readonly IConnectionService _connectionService;
        private readonly IEventAggregator _eventAggregator;
        private readonly ConnectionRecord _record;

        public ConnectionViewModel(IAgentProvider agentProvider, 
            IMessageService messageService,
            IDiscoveryService discoveryService,
            IConnectionService connectionService,
            IEventAggregator eventAggregator, 
            ConnectionRecord record) 
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


        public void OnNavigatingFrom()
        {
            throw new NotImplementedException();
        }

        public void OnNavigatingTo()
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedTo()
        {
            throw new NotImplementedException();
        }


        #region Bindable Properties
        private string _connectionName;
        public string ConnectionName
        {
            get => _connectionName;
            set => this.RaiseAndUpdate(ref _connectionName, value);
        }

        private string _myDid;
        public string MyDid
        {
            get => _myDid;
            set => this.RaiseAndUpdate(ref _myDid, value);
        }

        private string _theirDid;
        public string TheirDid
        {
            get => _theirDid;
            set => this.RaiseAndUpdate(ref _theirDid, value);
        }

        private string _connectionImageUrl;
        public string ConnectionImageUrl
        {
            get => _connectionImageUrl;
            set => this.RaiseAndUpdate(ref _connectionImageUrl, value);
        }

        private string _connectionSubtitle = "Lorem ipsum dolor sit amet";
        public string ConnectionSubtitle
        {
            get => _connectionSubtitle;
            set => this.RaiseAndUpdate(ref _connectionSubtitle, value);
        }

        private ObservableCollection<TransactionItem> _transactions = new ObservableCollection<TransactionItem>();
        public ObservableCollection<TransactionItem> Transactions
        {
            get => _transactions;
            set => this.RaiseAndUpdate(ref _transactions, value);
        }

        private bool _refreshingTransactions;
        public bool RefreshingTransactions
        {
            get => _refreshingTransactions;
            set => this.RaiseAndUpdate(ref _refreshingTransactions, value);
        }

        private bool _hasTransactions;
        public bool HasTransactions
        {
            get => _hasTransactions;
            set => this.RaiseAndUpdate(ref _hasTransactions, value);
        }

        #endregion
    }
}
