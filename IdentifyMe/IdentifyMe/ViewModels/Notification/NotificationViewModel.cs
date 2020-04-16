using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Features.IssueCredential;
using Hyperledger.Aries.Features.PresentProof;
using Hyperledger.Aries.Storage;
using IdentifyMe.Framework.Services;
using IdentifyMe.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IdentifyMe.ViewModels.Notification
{
    public class NotificationViewModel : BaseNavigationViewModel, INavigationAware
    {

        ICredentialService _credentialService;
        IConnectionService _connectionService;
        IProofService _proofService;
        IAgentProvider _agentProvider;
        private CloudWalletService _cloudWalletService;

        public NotificationViewModel(IAgentProvider agentProvider,
            ICredentialService credentialService, 
            IConnectionService connectionService,
            IMessageService messageService,
            CloudWalletService cloudWalletService,
            IProofService proofService)
        {
            _agentProvider = agentProvider;
            _credentialService = credentialService;
            _proofService = proofService;
            _connectionService = connectionService;
            _cloudWalletService = cloudWalletService;
            Title = "Notification";
        }

        public override async Task InitAsync()
        {
            await base.InitAsync();
            Console.WriteLine("Init Async work");

            await this.GetRequiredRecord();
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
            foreach (var item in listCredentials)
            {
                _listRecords.Add(item);
                _listCredOffer.Add(item);
                //CredOfferViewModel credentialVm = MakeVm<CredOfferViewModel>();
                //credentialVm.CredentialOffer = item;
                //_credentialOffersVm.Add(credentialVm);
            }



            foreach(var item in listProofRequests)
            {
                _listRecords.Add(item);
                _listProofRequest.Add(item);
                ProofRequestViewModel proofRequestVm = MakeVm<ProofRequestViewModel>();
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

        public async Task NavigateToCredentialOfferPage(CredentialRecord record)
        {
            //await Navigation.PushModalAsync(credOfferViewModel);
            //await Navigation.PushAsync(MakeVm<CredOfferViewModel>(credOfferViewModel));
            CredOfferViewModel credentialVm = MakeVm<CredOfferViewModel>();
            credentialVm.CredentialOffer = record;
            //_credentialOffersVm.Add(credentialVm);
            //await DisplayAlert("Alert", "You have been alerted", "OK");
            await Navigation.PushAsync(credentialVm);

           // await Application.Current.MainPage.DisplayAlert(credentialVm.CredentialOffer.ConnectionId, "", "Ok");

            //await Navigation.PushAsync(credentialVm);
            //await Navigation.PushAsync(MakeVm<ScanCodeViewModel>());


        }

        public ICommand SelectCredOfferCommand => new Command<CredentialRecord>(async (credOfferViewModel) =>
        {
            if (credOfferViewModel != null)
                await NavigateToCredentialOfferPage(credOfferViewModel);
            Console.WriteLine("Converted Worked");
        });

        public async Task NavigateToProofRequestPage(ProofRequestViewModel proofRequestViewModel)
        {
            await Navigation.PushAsync(proofRequestViewModel);
        }

        public ICommand SelectProofRequestCommand => new Command<ProofRequestViewModel>(async (proofRequestViewModel) =>
        {
            if (proofRequestViewModel != null)
                await NavigateToProofRequestPage(proofRequestViewModel);
        });
        //public async Task SelectCredential(CredentialViewModel credential) => await NavigationService.NavigateToAsync(credential, null, NavigationType.Modal);


        #region Bindable Props 

        private ObservableCollection<CredOfferViewModel> _credentialOffersVm = new ObservableCollection<CredOfferViewModel>();
        public ObservableCollection<CredOfferViewModel> CredOffers
        {
            get => _credentialOffersVm;
            set => this.RaiseAndUpdate(ref _credentialOffersVm, value);
        }
        
        private ObservableCollection<ProofRequestViewModel> _proofRequestsVm = new ObservableCollection<ProofRequestViewModel>();
        public ObservableCollection<ProofRequestViewModel> ProofRequests
        {
            get => _proofRequestsVm;
            set => this.RaiseAndUpdate(ref _proofRequestsVm, value);
        }

        private ObservableCollection<RecordBase> _listRecords = new ObservableCollection<RecordBase>();
        public ObservableCollection<RecordBase> ListRecord
        {
            get => _listRecords;
            set => RaiseAndUpdate(ref _listRecords, value);
        }

        private ObservableCollection<CredentialRecord> _listCredOffer = new ObservableCollection<CredentialRecord>();
        public ObservableCollection<CredentialRecord> ListCredOffer
        {
            get => _listCredOffer;
            set => RaiseAndUpdate(ref _listCredOffer, value);
        }
        
        private ObservableCollection<RecordBase> _listProofRequest = new ObservableCollection<RecordBase>();
        public ObservableCollection<RecordBase> ListProofRequest
        {
            get => _listProofRequest;
            set => RaiseAndUpdate(ref _listProofRequest, value);
        }

        private bool _isRefreshing = false;

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => RaiseAndUpdate(ref _isRefreshing, value);
        }
        #endregion

        #region Bindable Command
        public ICommand FetchInboxCommand => new Command(async () => await FetchInbox());

        public ICommand RefreshRecordCommand => new Command(async () => await GetRequiredRecord());
#endregion
    }
}
