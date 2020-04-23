using Acr.UserDialogs;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Features.IssueCredential;
using Hyperledger.Aries.Features.PresentProof;
using IdentifyMe.Framework.Services;
using IdentifyMe.Services.Interfaces;

namespace IdentifyMe.ViewModels.Notification
{
    public class NotificationViewModelV2 : ABaseViewModel
    {
        private ICredentialService _credentialService;
        private IConnectionService _connectionService;
        private IProofService _proofService;
        private IAgentProvider _agentProvider;
        private CloudWalletService _cloudWalletService;

        public NotificationViewModelV2(IUserDialogs userDialogs,
                                   INavigationServiceV2 navigationService,
                                   IAgentProvider agentProvider,
            ICredentialService credentialService,
            IConnectionService connectionService,
            IMessageService messageService,
            CloudWalletService cloudWalletService,
            IProofService proofService) : 
            base ("Notification", userDialogs, navigationService)
        {
            _agentProvider = agentProvider;
            _credentialService = credentialService;
            _proofService = proofService;
            _connectionService = connectionService;
            _cloudWalletService = cloudWalletService;
        }
    }
}
