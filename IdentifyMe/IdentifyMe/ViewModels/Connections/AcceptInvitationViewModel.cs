using Acr.UserDialogs;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Configuration;
using Hyperledger.Aries.Features.DidExchange;
using IdentifyMe.MVVM;
using IdentifyMe.MVVM.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IdentifyMe.ViewModels.Connections
{

    public class AcceptInvitationViewModel : BaseNavigationViewModel, IPopupViewModel
    {
        private ConnectionInvitationMessage _invitation;
        private readonly IConnectionService _connectionService;
        private readonly IMessageService _messageService;
        private readonly IProvisioningService _provisioningService;
        private readonly IAgentProvider _mobileAgentProvider;
        private IUserDialogs _userDialogs { get; }
        public AcceptInvitationViewModel(
           IConnectionService connectionService,
           IProvisioningService provisioningService,
           IMessageService messageService,
           IAgentProvider mobileAgentProvider
            )
        {

            _userDialogs = UserDialogs.Instance;
            _mobileAgentProvider = mobileAgentProvider;
            _provisioningService = provisioningService;
            _connectionService = connectionService;
            _messageService = messageService;
        }

        private async Task CreateConnection(IAgentContext context, ConnectionInvitationMessage invite)
        {
            var provisioningRecord = await _provisioningService.GetProvisioningAsync(context.Wallet);
            var isEndpointUriAbsent = provisioningRecord.Endpoint.Uri == null;
            var (msg, rec) = await _connectionService.CreateRequestAsync(context, _invitation);
            var rsp = await _messageService.SendReceiveAsync<ConnectionResponseMessage>(context.Wallet, msg, rec);
            if (isEndpointUriAbsent)
            {
                await _connectionService.ProcessResponseAsync(context, rsp, rec);
            }

        }
        private async Task AcceptInvitation()
        {
            var loadingDialog = _userDialogs.Loading("Proccessing");

            if (_invitation != null)
            {
                try
                {
                    var agentContext = await _mobileAgentProvider.GetContextAsync();
                    if (agentContext == null)
                    {
                        loadingDialog.Hide();
                        _userDialogs.Alert("Failed to decode invitation!");
                        return;
                    }
                    var (requestMessage, connectionRecord) = await _connectionService.CreateRequestAsync(agentContext, _invitation);
                    var provisioningRecord = await _provisioningService.GetProvisioningAsync(agentContext.Wallet);
                    var isEndpointUriAbsent = provisioningRecord.Endpoint.Uri == null;

                    var respone = await _messageService.SendReceiveAsync<ConnectionResponseMessage>(agentContext.Wallet, requestMessage, connectionRecord);
                    if (isEndpointUriAbsent)
                    {
                        string processRes = await _connectionService.ProcessResponseAsync(agentContext, respone, connectionRecord);
                    }
   
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    loadingDialog.Hide();
                    _userDialogs.Alert("Something went wrong!");
                }
                loadingDialog.Hide();
                await Navigation.PopPopupAsync();
                var toastConfig = new ToastConfig("Connection Saved!");
                toastConfig.BackgroundColor = Color.Green;
                toastConfig.Position = ToastPosition.Top;
                toastConfig.SetDuration(3000);
                _userDialogs.Toast(toastConfig);
            }

        }

        #region Binding Props
        public ConnectionInvitationMessage InvitationMessage
        {
            get => _invitation;
            set => RaiseAndUpdate(ref _invitation, value);
        }
        #endregion
        #region Binding Command
        public ICommand AcceptInvitationCommand => new Command(async () => await AcceptInvitation());
        #endregion
    }

}
