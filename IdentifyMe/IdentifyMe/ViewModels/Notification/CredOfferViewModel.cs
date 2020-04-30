using Acr.UserDialogs;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Contracts;
using Hyperledger.Aries.Extensions;
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Features.IssueCredential;
using Hyperledger.Aries.Storage;
using IdentifyMe.Events;
using IdentifyMe.Framework.Services;
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
    public class CredOfferViewModel : ABaseViewModel
    {
        private readonly IAgentProvider _agentProvider;
        private readonly ICredentialService _credentialService;
        private readonly IConnectionService _connectionService;
        private readonly IMessageService _messageService;
        private readonly IPoolService _poolService;
        private readonly IWalletRecordService _recordService;
        private readonly IEventAggregator _eventAggregator;

        public CredOfferViewModel(IUserDialogs userDialogs,
            INavigationService navigationService, 
            IAgentProvider agentProvider,
            ICredentialService credentialService,
            IMessageService messageService,
            IPoolService poolService,
            IWalletRecordService recordService,
            IConnectionService connectionService, 
            IEventAggregator eventAggregator) :
            base(nameof(CredOfferViewModel), userDialogs, navigationService)
        {
            _agentProvider = agentProvider;
            _credentialService = credentialService;
            _connectionService = connectionService;
            _messageService = messageService;
            _poolService = poolService;
            _recordService = recordService;
            _eventAggregator = eventAggregator;
            Title = "Offer Detail";
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
                var loadingDialog = DialogService.Loading("Proccessing");
                try
                {
                    var poolConfigName = Preferences.Get(Constants.PoolConfigurationName, "sovrin-staging");    
                    var a = await _poolService.GetPoolAsync(poolConfigName, 2);
                    var (requestMessage, credRecord) = await _credentialService.CreateRequestAsync(context, CredentialOffer.Id);
                    var connectionRecord  = await _connectionService.GetAsync(context, credRecord.ConnectionId);
                    await _messageService.SendAsync(context.Wallet, requestMessage, connectionRecord);      
                    loadingDialog.Hide();
                    await NavigationService.NavigateBackAsync();
                    var toastConfig = new ToastConfig("Accepted Credential Offer");
                    toastConfig.BackgroundColor = Color.Green;
                    toastConfig.Position = ToastPosition.Top;
                    toastConfig.SetDuration(3000);
                    DialogService.Toast(toastConfig);
                    _eventAggregator.Publish(new ApplicationEvent() { Type = ApplicationEventType.CredentialsUpdated });
                }
                catch (Exception e)
                {
                    loadingDialog.Hide();
                    await NavigationService.NavigateBackAsync();
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
