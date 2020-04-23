using Acr.UserDialogs;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Features.IssueCredential;
using IdentifyMe.Services.Interfaces;
using ReactiveUI;

namespace IdentifyMe.ViewModels.Credentials
{
    public class CredentialsViewModelV2 : ABaseViewModel
    {
        private readonly IAgentProvider _agentProvider;
        private readonly ICredentialService _credentialService;

        public CredentialsViewModelV2(IUserDialogs userDialogs,
                                  INavigationServiceV2 navigationService,
                                  IAgentProvider agentProvider,
                                  ICredentialService credentialService ) : 
            base("Credentials", userDialogs, navigationService)
        {
            _agentProvider = agentProvider;
            _credentialService = credentialService;
        }

        string _test;
        public string Test
        {
            get => _test;
            set => this.RaiseAndSetIfChanged(ref _test, value);
        }
    }
}
