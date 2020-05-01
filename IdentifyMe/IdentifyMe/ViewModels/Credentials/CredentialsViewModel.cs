using Acr.UserDialogs;
using Autofac;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Features.IssueCredential;
using IdentifyMe.Extensions;
using IdentifyMe.Services.Interfaces;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IdentifyMe.ViewModels.Credentials
{
    public class CredentialsViewModel : ABaseViewModel
    {
        private readonly IAgentProvider _agentProvider;
        private readonly ICredentialService _credentialService;
        private readonly ILifetimeScope _scope;

        public CredentialsViewModel(IUserDialogs userDialogs,
                                  INavigationService navigationService,
                                  IAgentProvider agentProvider,
                                  ICredentialService credentialService, 
                                  ILifetimeScope scope ) : 
            base(nameof(CredentialsViewModel), userDialogs, navigationService)
        {
            _agentProvider = agentProvider;
            _credentialService = credentialService;
            _scope = scope;
            Title = "Credentials";
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
            var credentialRecordsList = await _credentialService.ListAsync(context);
            if (credentialRecordsList != null)
            {
                _credentialVm.Clear();
                foreach (var record in credentialRecordsList)
                {
                    //_listRecords.Add(item);
                    //_listProofRequest.Add(item);
                    CredentialViewModel credViewModel = _scope.Resolve<CredentialViewModel>(new NamedParameter("credential", record));
                    //credViewModel.CredentialRecord = item;
                    _credentialVm.Add(credViewModel);
                    var a = record.SchemaId;
                }
            }

            IsRefreshing = false;
        }

        private RangeEnabledObservableCollection<CredentialViewModel> _credentialVm = new RangeEnabledObservableCollection<CredentialViewModel>();

        public RangeEnabledObservableCollection<CredentialViewModel> CredentialViewModels
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

        public ICommand SelectCredentialCommand => new Command<CredentialViewModel>(async (credentialVm) => {
            if(credentialVm != null)
            {
               await NavigationService.NavigateToAsync<CredentialViewModel>(credentialVm);
            }
        });

    }
}
