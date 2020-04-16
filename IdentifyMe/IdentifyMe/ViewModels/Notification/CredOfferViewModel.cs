using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Extensions;
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Features.IssueCredential;
using IdentifyMe.MVVM;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;


namespace IdentifyMe.ViewModels.Notification
{
    public class CredOfferViewModel : BaseNavigationViewModel, INavigationAware
    {
        private IAgentProvider _agentProvider;
        private ICredentialService _credentialService;
        private IConnectionService _connectionService;
        private IMessageService _messageService;

        public CredOfferViewModel(IAgentProvider agentProvider, 
            ICredentialService credentialService, 
            IMessageService messageService,
            IConnectionService connectionService)
        {
            Title = "Credential Offer";
           _agentProvider = agentProvider;
           _credentialService = credentialService;
            _connectionService = connectionService;
            _messageService = messageService;
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
                RaiseAndUpdate(ref _credentialOffer, value);
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
            set => RaiseAndUpdate(ref _credentialAttributes, value);
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
                    var connection = await _connectionService.GetAsync(context, CredentialOffer.ConnectionId);
                    var (request, record) = await _credentialService.CreateRequestAsync(context, this.CredentialOffer.Id);
                    await Application.Current.MainPage.DisplayAlert("Accepted", "", "Ok");
                    var result = _messageService.SendReceiveAsync(context.Wallet, request, connection);
                }
                catch (Exception e)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "", "Ok");
                    Console.WriteLine($"Error: {e.Message}");
                }


            }
            //await _messageService.SendAsync<CredentialProposeMessage>()
            await Application.Current.MainPage.DisplayAlert("Something error with this Offer", "", "Ok");

        }
        public ICommand RejectCredentialOfferCommand => new Command(async () => await Application.Current.MainPage.DisplayAlert("Receject Credential Offer", "", "OK"));
        public ICommand AcceptCredentialOfferCommand => new Command(async () => await Application.Current.MainPage.DisplayAlert("Accept Credential Offer", "", "Ok"));
        #endregion
    }
}
