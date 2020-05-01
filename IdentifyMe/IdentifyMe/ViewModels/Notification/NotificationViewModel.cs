using Acr.UserDialogs;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Features.IssueCredential;
using Hyperledger.Aries.Features.PresentProof;
using IdentifyMe.Framework.Services;
using IdentifyMe.Messages;
using IdentifyMe.Services.Interfaces;
using Plugin.LocalNotification;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Hyperledger.Aries.Storage;
using System.Linq;
using Autofac;
using IdentifyMe.Extensions;
using Hyperledger.Aries.Contracts;
using IdentifyMe.Events;
using System.Reactive.Linq;

namespace IdentifyMe.ViewModels.Notification
{
    public class NotificationViewModel : ABaseViewModel
    {
        private ICredentialService _credentialService;
        private IConnectionService _connectionService;
        private IProofService _proofService;
        private IAgentProvider _agentProvider;
        private CloudWalletService _cloudWalletService;
        private readonly ILifetimeScope _scope;
        private readonly IEventAggregator _eventAggregator;
        public NotificationViewModel(IUserDialogs userDialogs,
                                   INavigationService navigationService,
                                   IAgentProvider agentProvider,
                                   ICredentialService credentialService,
                                   IConnectionService connectionService,
                                   IMessageService messageService,
                                   CloudWalletService cloudWalletService,
                                   IProofService proofService, 
                                   ILifetimeScope scope, 
                                   IEventAggregator eventAggregator) : base (nameof(NotificationViewModel), userDialogs, navigationService)
        {
            _agentProvider = agentProvider;
            _credentialService = credentialService;
            _proofService = proofService;
            _connectionService = connectionService;
            _cloudWalletService = cloudWalletService;
            _scope = scope;
            _eventAggregator = eventAggregator;
            Title = "Notification";
        }

        public async override Task InitializeAsync(object navigationData)
        {
           
            await base.InitializeAsync(navigationData);
            _eventAggregator.GetEventByType<ApplicationEvent>()
               .Where(_ => _.Type == ApplicationEventType.CredentialsUpdated)
               .Subscribe(async _ => await GetRequiredRecord());
            _eventAggregator.GetEventByType<ApplicationEvent>()
                .Where(_ => _.Type == ApplicationEventType.GotCredentialOffer)
                .Subscribe(async _ => await GetRequiredRecord());
            _eventAggregator.GetEventByType<ApplicationEvent>()
                .Where(_ => _.Type == ApplicationEventType.GotProofRequestMessage)
                .Subscribe(async _ => await GetRequiredRecord());
            await GetRequiredRecord();
        }

        void HandleReceivedMessages()
        {
            MessagingCenter.Subscribe<TickedMessage>(this, "TickedMessage", message => {
                Device.BeginInvokeOnMainThread(async () => {
                    var num = await _cloudWalletService.FetchCloudMessagesAsync();
                    if (num != 0)
                    {
                        var notification = new NotificationRequest
                        {
                            NotificationId = 100,
                            Title = "Message from mediator",
                            Description = $"You've got {num} message from mediator",
                            ReturningData = "Dummy data", // Returning data when tapped on notification.
                                                          //NotifyTime = DateTime.Now.AddSeconds(30) // Used for Scheduling local notification, if not specified notification will show immediately.
                        };
                        NotificationCenter.Current.Show(notification);
                    }
                });
            });

        }

        private async Task GetRequiredRecord()
        {
            IsRefreshing = true;
            var agentContext = await _agentProvider.GetContextAsync();
            //ISearchQuery credentialsQuery = ListOffersAsync
            var listCredentials = await _credentialService.ListOffersAsync(agentContext);

            var listProofRequests = await _proofService.ListRequestedAsync(agentContext);
            //List<RecordBase> list = new List<RecordBase>();
            _listRecords.Clear();
            _listCredOffer.Clear();
            _listProofRequest.Clear();
            _proofRequestsVm.Clear();
            foreach (var item in listCredentials)
            {
                _listRecords.Add(item);
                _listCredOffer.Add(item);
                //CredOfferViewModel credentialVm = MakeVm<CredOfferViewModel>();
                //credentialVm.CredentialOffer = item;
                //_credentialOffersVm.Add(credentialVm);
            }

            foreach (var item in listProofRequests)
            {
                _listRecords.Add(item);
                _listProofRequest.Add(item);
                ProofRequestViewModel proofRequestVm = _scope.Resolve<ProofRequestViewModel>();
                proofRequestVm.ProofRequestRecord = item;
                _proofRequestsVm.Add(proofRequestVm);
            }
            _listRecords.OrderBy(x => x.CreatedAtUtc);
            IsRefreshing = false;
        }

        private async Task FetchInbox()
        {
            try
            {
                var messageCount = await _cloudWalletService.FetchCloudMessagesAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public async Task NavigateToCredentialOfferPage(CredentialRecord record)
        {
            //await Navigation.PushModalAsync(credOfferViewModel);
            //await Navigation.PushAsync(MakeVm<CredOfferViewModel>(credOfferViewModel));
            CredOfferViewModel credentialVm = _scope.Resolve<CredOfferViewModel>();
            credentialVm.CredentialOffer = record;
            //_credentialOffersVm.Add(credentialVm);
            //await DisplayAlert("Alert", "You have been alerted", "OK");
            await NavigationService.NavigateToAsync<CredOfferViewModel>(credentialVm);
        }

        public ICommand SelectCredOfferCommand => new Command<CredentialRecord>(async (credOfferViewModel) =>
        {
            if (credOfferViewModel != null)
                await NavigateToCredentialOfferPage(credOfferViewModel);
        });

        public async Task NavigateToProofRequestPage(ProofRequestViewModel proofRequestViewModel)
        {
            await NavigationService.NavigateToAsync<ProofRequestViewModel>(proofRequestViewModel);
        }

        public ICommand SelectProofRequestCommand => new Command<ProofRequestViewModel>(async (proofRequestViewModel) =>
        {
            if (proofRequestViewModel != null)
                await NavigateToProofRequestPage(proofRequestViewModel);
        });

        #region Bindable Props 

        private RangeEnabledObservableCollection<CredOfferViewModel> _credentialOffersVm = new RangeEnabledObservableCollection<CredOfferViewModel>();
        public RangeEnabledObservableCollection<CredOfferViewModel> CredOffers
        {
            get => _credentialOffersVm;
            set => this.RaiseAndSetIfChanged(ref _credentialOffersVm, value);
        }

        private RangeEnabledObservableCollection<ProofRequestViewModel> _proofRequestsVm = new RangeEnabledObservableCollection<ProofRequestViewModel>();
        public RangeEnabledObservableCollection<ProofRequestViewModel> ProofRequests
        {
            get => _proofRequestsVm;
            set => this.RaiseAndSetIfChanged(ref _proofRequestsVm, value);
        }

        private RangeEnabledObservableCollection<RecordBase> _listRecords = new RangeEnabledObservableCollection<RecordBase>();
        public RangeEnabledObservableCollection<RecordBase> ListRecord
        {
            get => _listRecords;
            set => this.RaiseAndSetIfChanged(ref _listRecords, value);
        }

        private RangeEnabledObservableCollection<CredentialRecord> _listCredOffer = new RangeEnabledObservableCollection<CredentialRecord>();
        public RangeEnabledObservableCollection<CredentialRecord> ListCredOffer
        {
            get => _listCredOffer;
            set => this.RaiseAndSetIfChanged(ref _listCredOffer, value);
        }

        private RangeEnabledObservableCollection<RecordBase> _listProofRequest = new RangeEnabledObservableCollection<RecordBase>();
        public RangeEnabledObservableCollection<RecordBase> ListProofRequest
        {
            get => _listProofRequest;
            set => this.RaiseAndSetIfChanged(ref _listProofRequest, value);
        }

        private bool _isRefreshing = false;

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => this.RaiseAndSetIfChanged(ref _isRefreshing, value);
        }
        #endregion

        #region Bindable Command
        public ICommand FetchInboxCommand => new Command(async () => await FetchInbox());

        public ICommand RefreshRecordCommand => new Command(async () => await GetRequiredRecord());
        #endregion

    }
}
