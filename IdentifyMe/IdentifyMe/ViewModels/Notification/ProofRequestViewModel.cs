using Acr.UserDialogs;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Extensions;
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Features.IssueCredential;
using Hyperledger.Aries.Features.PresentProof;
using Hyperledger.Indy.AnonCredsApi;
using IdentifyMe.Extensions;
using IdentifyMe.Services.Interfaces;
using Newtonsoft.Json.Linq;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IdentifyMe.ViewModels.Notification
{
    public class ProofRequestViewModel : ABaseViewModel
    {
        private readonly IProofService _proofService;
        private readonly IAgentProvider _agentProvider;
        private readonly IMessageService _messageService;
        private readonly IConnectionService _connectionService;
        private readonly ProofRecord _proofRecord;
        public ProofRequestViewModel(IUserDialogs userDialogs,
            INavigationService navigationService, 
            IProofService proofService, 
            IAgentProvider agentProvider, 
            IMessageService messageService, 
            IConnectionService connectionService, ProofRecord proofRequestRecord) : base (nameof(ProofRequestViewModel), userDialogs, navigationService)
        {
            Title = "Proof Request";
            _proofService = proofService;
            _agentProvider = agentProvider;
            _messageService = messageService;
            _connectionService = connectionService;
            _proofRequestRecord = proofRequestRecord;
            _proofRequest = _proofRequestRecord.RequestJson.ToObject<ProofRequest>();
        }

        public async override Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            await CreateRequestedCredential();
            await base.InitializeAsync(navigationData);
            IsBusy = false;
        }

        // private RangeEnabledObservableCollection<KeyValuePair<string, string>> _roofRequestCredentialsPairs = new RangeEnabledObservableCollection<KeyValuePair<string, string>>();

        //Dictionary<string, string> proofAndCredentialPredicatesMapping = new Dictionary<string, string>();
        private async Task CreateRequestedCredential()
        {
            var requestedCredentials = new RequestedCredentials();
            var context = await _agentProvider.GetContextAsync();
            _proofRequestAndCredentialMaps.Clear();
            RangeEnabledObservableCollection<ProofRequestAndCredentialMap> proofRequestMapList = new RangeEnabledObservableCollection<ProofRequestAndCredentialMap>();
            foreach (var requestedAttribute in ProofRequestObject.RequestedAttributes)
            {
                ProofRequestAndCredentialMap proofCredMap = new ProofRequestAndCredentialMap();

                proofCredMap.ProofKey = requestedAttribute.Key;

                var credentials = await _proofService.ListCredentialsForProofRequestAsync(context, _proofRequest,
                        requestedAttribute.Key);
                proofCredMap.Referent = credentials.First().CredentialInfo.Referent;

                var key = requestedAttribute.Value.Name;
                if (credentials.First().CredentialInfo.Attributes.ContainsKey(key))
                {
                    var value = credentials.First().CredentialInfo.Attributes[key];
                    KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>(key, value);
                    proofCredMap.CredentialAttribute = keyValuePair;
                }

                requestedCredentials.RequestedAttributes.Add(requestedAttribute.Key,
                    new RequestedAttribute
                    {
                        CredentialId = credentials.First().CredentialInfo.Referent,
                        Revealed = true
                    });
                proofRequestMapList.Add(proofCredMap);
                //requestedCredentials.RequestedAttributes.
                //proofAndCredentialAttributesMapping.Add(requestedAttribute, credentials.First().CredentialInfo.Attributes.)
            }

            foreach (var requestedAttribute in ProofRequestObject.RequestedPredicates)
            {
                var credentials = await _proofService.ListCredentialsForProofRequestAsync(context, ProofRequestObject,
                        requestedAttribute.Key);
                ProofRequestAndCredentialMap proofCredMap = new ProofRequestAndCredentialMap();
                proofCredMap.ProofKey = requestedAttribute.Key;
                proofCredMap.Referent = credentials.First().CredentialInfo.Referent;

                var key = requestedAttribute.Value.Name;
                if (credentials.First().CredentialInfo.Attributes.ContainsKey(key))
                {
                    var value = credentials.First().CredentialInfo.Attributes[key];
                    KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>(key, value);
                    proofCredMap.CredentialAttribute = keyValuePair;
                }
                requestedCredentials.RequestedPredicates.Add(requestedAttribute.Key,
                    new RequestedAttribute
                    {
                        CredentialId = credentials.First().CredentialInfo.Referent,
                        Revealed = true
                    });
                proofRequestMapList.Add(proofCredMap);
            }
            ProofRequestAndCredentialMaps = proofRequestMapList;
            RequestedCredentials = requestedCredentials;
        }


        private async Task AcceptProofRequest()
        {
            var loadingDialog = DialogService.Loading("Proccessing");
            try
            {

                this.IsBusy = true;
                var context = await _agentProvider.GetContextAsync();
                var (message, proofRecord) = await _proofService.CreatePresentationAsync(context, ProofRequestRecord.Id, RequestedCredentials);
                var connectionRecord = await _connectionService.GetAsync(context, proofRecord.ConnectionId);
                await _messageService.SendAsync(context.Wallet, message, connectionRecord);
                loadingDialog.Hide();
                this.IsBusy = false;
                await NavigationService.NavigateBackAsync();
                var toastConfig = new ToastConfig("Accepted Proof!");
                toastConfig.BackgroundColor = Color.Green;
                toastConfig.Position = ToastPosition.Top;
                toastConfig.SetDuration(3000);
                DialogService.Toast(toastConfig);
            }
            catch (Exception e)
            {
                this.IsBusy = false;
                loadingDialog.Hide();
                DialogService.Alert("Error while accept Proof Request");
            }

        }

        private async Task RejectProofRequest()
        {
            var loadingDialog = DialogService.Loading("Proccessing");
            try
            {

                this.IsBusy = true;
                var context = await _agentProvider.GetContextAsync();
                // var (message, proofRecord) = await _proofService.CreatePresentationAsync(context, ProofRequestRecord.Id, RequestedCredentials);
                await _proofService.RejectProofRequestAsync(context, ProofRequestRecord.Id);
                loadingDialog.Hide();
                this.IsBusy = false;
                await NavigationService.CloseAllPopupsAsync();
                var toastConfig = new ToastConfig("Rejected Proof!");
                toastConfig.BackgroundColor = Color.Green;
                toastConfig.Position = ToastPosition.Top;
                toastConfig.SetDuration(3000);
                DialogService.Toast(toastConfig);
            }
            catch (Exception e)
            {
                this.IsBusy = false;
                loadingDialog.Hide();
                DialogService.Alert("Error while accept Proof Request");
            }

        }


        #region Bindable Props 
        private ProofRecord _proofRequestRecord;

        public ProofRecord ProofRequestRecord
        {
            get => _proofRequestRecord;
            set
            {
                this.RaiseAndSetIfChanged(ref _proofRequestRecord, value);
            }
        }

        private ProofRequest _proofRequest;
        public ProofRequest ProofRequestObject
        {
            get => this._proofRequest;
            set => this.RaiseAndSetIfChanged(ref _proofRequest, value);
        }

        private RequestedCredentials _requestedCredentials;
        public RequestedCredentials RequestedCredentials
        {
            get => this._requestedCredentials;
            set => this.RaiseAndSetIfChanged(ref _requestedCredentials, value);
        }

        private RangeEnabledObservableCollection<ProofRequestAndCredentialMap> _proofRequestAndCredentialMaps = new RangeEnabledObservableCollection<ProofRequestAndCredentialMap>();
        public RangeEnabledObservableCollection<ProofRequestAndCredentialMap> ProofRequestAndCredentialMaps
        {
            get => _proofRequestAndCredentialMaps;
            set => this.RaiseAndSetIfChanged(ref _proofRequestAndCredentialMaps, value);
        }
        #endregion

        #region Bindable Command 
        public ICommand AcceptProofRequestCommand => new Command(async () => await AcceptProofRequest());
        public ICommand RejectProofRequestCommand => new Command(async () => await Application.Current.MainPage.DisplayAlert("Rejected Proof Request", "", "Ok"));
        #endregion
    }
}
