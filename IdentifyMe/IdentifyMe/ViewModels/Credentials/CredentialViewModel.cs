using Acr.UserDialogs;
using Hyperledger.Aries.Features.IssueCredential;
using IdentifyMe.Services.Interfaces;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentifyMe.ViewModels.Credentials
{
    public class CredentialViewModel : ABaseViewModel
    {

        public CredentialViewModel(IUserDialogs userDialogs,
            INavigationService navigationService) : 
            base (nameof(CredentialViewModel), userDialogs, navigationService )
        {
            Title = "Credential Detail";
        }
     

        private CredentialRecord _credentialRecord;

        public CredentialRecord CredentialRecord
        {
            get => _credentialRecord;
            set
            {
                this.RaiseAndSetIfChanged(ref _credentialRecord, value);
                _credId = _credentialRecord.Id;
                _state = _credentialRecord.State;

            }
        }

        private string _credId;

        public string Id
        {
            get => _credId;
            set => this.RaiseAndSetIfChanged(ref _credId, value);
        }

        private CredentialState _state;

        public CredentialState State
        {
            get => _state;
            set => this.RaiseAndSetIfChanged(ref _state, value);
        }
    }
}
