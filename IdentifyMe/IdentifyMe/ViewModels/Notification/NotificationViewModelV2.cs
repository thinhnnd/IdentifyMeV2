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

namespace IdentifyMe.ViewModels.Notification
{
    public class NotificationViewModelV2 : ABaseViewModel
    {
        private ICredentialService _credentialService;
        private IConnectionService _connectionService;
        private IProofService _proofService;
        private IAgentProvider _agentProvider;
        private CloudWalletService _cloudWalletService;
        private readonly ILifetimeScope _scope;

        public NotificationViewModelV2(IUserDialogs userDialogs,
                                   INavigationServiceV2 navigationService,
                                   IAgentProvider agentProvider,
                                   ICredentialService credentialService,
                                   IConnectionService connectionService,
                                   IMessageService messageService,
                                   CloudWalletService cloudWalletService,
                                   IProofService proofService, 
                                   ILifetimeScope scope) : base (nameof(NotificationViewModelV2), userDialogs, navigationService)
        {
            _agentProvider = agentProvider;
            _credentialService = credentialService;
            _proofService = proofService;
            _connectionService = connectionService;
            _cloudWalletService = cloudWalletService;
            _scope = scope;
            Title = "Notification";
        }

        public async override Task InitializeAsync(object navigationData)
        {
            await base.InitializeAsync(navigationData);
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
                ProofRequestViewModelV2 proofRequestVm = _scope.Resolve<ProofRequestViewModelV2>();
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
            CredOfferViewModelV2 credentialVm = _scope.Resolve<CredOfferViewModelV2>();
            credentialVm.CredentialOffer = record;
            //_credentialOffersVm.Add(credentialVm);
            //await DisplayAlert("Alert", "You have been alerted", "OK");
            await NavigationService.NavigateToAsync<CredOfferViewModelV2>(credentialVm);

            // await Application.Current.MainPage.DisplayAlert(credentialVm.CredentialOffer.ConnectionId, "", "Ok");

            //await Navigation.PushAsync(credentialVm);
            //await Navigation.PushAsync(MakeVm<ScanCodeViewModel>());


        }

        public ICommand SelectCredOfferCommand => new Command<CredentialRecord>(async (credOfferViewModel) =>
        {
            if (credOfferViewModel != null)
                await NavigateToCredentialOfferPage(credOfferViewModel);
        });

        public async Task NavigateToProofRequestPage(ProofRequestViewModelV2 proofRequestViewModel)
        {
            await NavigationService.NavigateToAsync<ProofRequestViewModelV2>(proofRequestViewModel);
        }

        public ICommand SelectProofRequestCommand => new Command<ProofRequestViewModelV2>(async (proofRequestViewModel) =>
        {
            if (proofRequestViewModel != null)
                await NavigateToProofRequestPage(proofRequestViewModel);
        });
        //public async Task SelectCredential(CredentialViewModel credential) => await NavigationService.NavigateToAsync(credential, null, NavigationType.Modal);


        #region Bindable Props 

        private ObservableCollection<CredOfferViewModelV2> _credentialOffersVm = new ObservableCollection<CredOfferViewModelV2>();
        public ObservableCollection<CredOfferViewModelV2> CredOffers
        {
            get => _credentialOffersVm;
            set => this.RaiseAndSetIfChanged(ref _credentialOffersVm, value);
        }

        private ObservableCollection<ProofRequestViewModelV2> _proofRequestsVm = new ObservableCollection<ProofRequestViewModelV2>();
        public ObservableCollection<ProofRequestViewModelV2> ProofRequests
        {
            get => _proofRequestsVm;
            set => this.RaiseAndSetIfChanged(ref _proofRequestsVm, value);
        }

        private ObservableCollection<RecordBase> _listRecords = new ObservableCollection<RecordBase>();
        public ObservableCollection<RecordBase> ListRecord
        {
            get => _listRecords;
            set => this.RaiseAndSetIfChanged(ref _listRecords, value);
        }

        private ObservableCollection<CredentialRecord> _listCredOffer = new ObservableCollection<CredentialRecord>();
        public ObservableCollection<CredentialRecord> ListCredOffer
        {
            get => _listCredOffer;
            set => this.RaiseAndSetIfChanged(ref _listCredOffer, value);
        }

        private ObservableCollection<RecordBase> _listProofRequest = new ObservableCollection<RecordBase>();
        public ObservableCollection<RecordBase> ListProofRequest
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
