using Acr.UserDialogs;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Contracts;
using Hyperledger.Aries.Extensions;
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Features.IssueCredential;
using Hyperledger.Aries.Storage;
using IdentifyMe.Framework.Services;
using IdentifyMe.MVVM;
using IdentifyMe.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace IdentifyMe.ViewModels.Notification
{
    public class CredOfferViewModelV2 : ABaseViewModel
    {
        private IAgentProvider _agentProvider;
        private ICredentialService _credentialService;
        private IConnectionService _connectionService;
        private IMessageService _messageService;
        private IPoolService _poolService;
        private IWalletRecordService _recordService;

        public CredOfferViewModelV2(IUserDialogs userDialogs,
            INavigationServiceV2 navigationService, 
            IAgentProvider agentProvider,
            ICredentialService credentialService,
            IMessageService messageService,
            IPoolService poolService,
            IWalletRecordService recordService,
            IConnectionService connectionService) :
            base("Credential Detail", userDialogs, navigationService)
        {
            _agentProvider = agentProvider;
            _credentialService = credentialService;
            _connectionService = connectionService;
            _messageService = messageService;
            _poolService = poolService;
            _recordService = recordService;
        }

        public void OnNavigatedTo()
        {
            Console.WriteLine("On Navigated To");
        }

        public void OnNavigatingFrom()
        {
            Console.WriteLine("On Navigating From");
        }

        public void OnNavigatingTo()
        {
            Console.WriteLine("On Navigating To");
        }

        #region Bindable Props 
        private CredentialRecord _credentialOffer;
        public CredentialRecord CredentialOffer
        {
            get => _credentialOffer;
            set
            {
                this.RaiseAndSetIfChanged(ref _credentialOffer, value);
                if(value != null)
                {
                    _credentialAttributes = value.CredentialAttributesValues;
                }

            }
        }

        private IEnumerable<CredentialPreviewAttribute> _credentialAttributes;

        public IEnumerable<CredentialPreviewAttribute> CredentialData
        {
            get => _credentialAttributes;
            set => this.RaiseAndSetIfChanged(ref _credentialAttributes, value);
        }
        #endregion

        #region Bindable Command
        private void RejectCredentialOffer ()
        {

        }

        private async Task AcceptCredentialOffer()
        {
            if(this.CredentialOffer != null)
            {
                var context = await _agentProvider.GetContextAsync();
                try
                {
                    var poolConfigName = Preferences.Get(Constants.PoolConfigurationName, "sovrin-staging");    
                    var a = await _poolService.GetPoolAsync(poolConfigName, 2);
                    var (requestMessage, credRecord) = await _credentialService.CreateRequestAsync(context, CredentialOffer.Id);
                    var connectionRecord  = await _connectionService.GetAsync(context, credRecord.ConnectionId);
                    var result = await _messageService.SendReceiveAsync(context.Wallet, requestMessage, connectionRecord);
                    await NavigationService.NavigateBackAsync();                   
                }
                catch (Exception e)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "", "Ok");
                    Console.WriteLine($"Error: {e.Message}");
                }


            }
            //await _messageService.SendAsync<CredentialProposeMessage>()
            //await Application.Current.MainPage.DisplayAlert("Something error with this Offer", "", "Ok");

        }
        public ICommand AcceptCredentialOfferCommand => new Command(async () => await AcceptCredentialOffer());
        public ICommand RejectCredentialOfferCommand => new Command(async () => await Application.Current.MainPage.DisplayAlert("Rejected Credential Offer", "", "Ok"));
        #endregion
    }
}
