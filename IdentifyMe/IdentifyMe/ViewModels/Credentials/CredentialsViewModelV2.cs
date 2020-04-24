using Acr.UserDialogs;
using Autofac;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Features.IssueCredential;
using IdentifyMe.Services.Interfaces;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IdentifyMe.ViewModels.Credentials
{
    public class CredentialsViewModelV2 : ABaseViewModel
    {
        private readonly IAgentProvider _agentProvider;
        private readonly ICredentialService _credentialService;
        private readonly ILifetimeScope _scope;

        public CredentialsViewModelV2(IUserDialogs userDialogs,
                                  INavigationServiceV2 navigationService,
                                  IAgentProvider agentProvider,
                                  ICredentialService credentialService, 
                                  ILifetimeScope scope ) : 
            base("Credentials", userDialogs, navigationService)
        {
            _agentProvider = agentProvider;
            _credentialService = credentialService;
            _scope = scope;
        }

        public async override Task InitializeAsync(object navigationData)
        {
            await base.InitializeAsync(navigationData);
            await LoadCredential();
        }

        string _test = "Worked";
        public string Test
        {
            get => _test;
            set => this.RaiseAndSetIfChanged(ref _test, value);
        }

        private async Task LoadCredential()
        {
            IsRefreshing = true;
            var context = await _agentProvider.GetContextAsync();
            var credentialRecordsList = await _credentialService.ListAsync(context, null, 100);
            if (credentialRecordsList != null)
            {
                _credentialVm.Clear();
                foreach (var item in credentialRecordsList)
                {
                    //_listRecords.Add(item);
                    //_listProofRequest.Add(item);
                    CredentialViewModelV2 credViewModel = _scope.Resolve<CredentialViewModelV2>();
                    credViewModel.CredentialRecord = item;
                    _credentialVm.Add(credViewModel);
                }
            }

            IsRefreshing = false;
        }

        private ObservableCollection<CredentialViewModelV2> _credentialVm = new ObservableCollection<CredentialViewModelV2>();

        public ObservableCollection<CredentialViewModelV2> CredentialViewModel
        {
            get => _credentialVm;
            set => this.RaiseAndSetIfChanged(ref _credentialVm, value);
        }


        public ICommand RefreshCredentialsCommand => new Command(async () => await LoadCredential());

        private bool _isRefreshing = false;

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => this.RaiseAndSetIfChanged(ref _isRefreshing, value);
        }
        //public ICommand SelectCredOfferCommand => new Command<CredentialRecord>(async (credOfferViewModel) =>
        //{
        //    if (credOfferViewModel != null)
        //        await NavigateToCredentialOfferPage(credOfferViewModel);
        //    Console.WriteLine("Converted Worked");
        //});
    }
}
