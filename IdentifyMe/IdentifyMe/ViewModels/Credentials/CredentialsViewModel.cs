using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Features.IssueCredential;
using IdentifyMe.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IdentifyMe.ViewModels.Credentials
{
    public class CredentialsViewModel : BaseNavigationViewModel
    {
        private readonly IAgentProvider _agentProvider;
        private readonly ICredentialService _credentialService;
        public CredentialsViewModel(IAgentProvider agentProvider, 
            ICredentialService credentialService)
        {
            _agentProvider = agentProvider;
            _credentialService = credentialService;
            Title = "Credentials";
            InitializeAsync();

        }

        public async void InitializeAsync()
        {
            await LoadCredential();
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
                    CredentialViewModel credViewModel = MakeVm<CredentialViewModel>();
                    credViewModel.CredentialRecord = item;
                    _credentialVm.Add(credViewModel);
                }
            }

            IsRefreshing = false;
        }

        private ObservableCollection<CredentialViewModel> _credentialVm = new ObservableCollection<CredentialViewModel>();

        public ObservableCollection<CredentialViewModel> CredentialViewModel
        {
            get => _credentialVm;
            set => RaiseAndUpdate(ref _credentialVm, value);
        }


        public ICommand RefreshCredentialsCommand => new Command(async () => await LoadCredential());

        private bool _isRefreshing = false;

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => RaiseAndUpdate(ref _isRefreshing, value);
        }
        //public ICommand SelectCredOfferCommand => new Command<CredentialRecord>(async (credOfferViewModel) =>
        //{
        //    if (credOfferViewModel != null)
        //        await NavigateToCredentialOfferPage(credOfferViewModel);
        //    Console.WriteLine("Converted Worked");
        //});
    }
}
