using Hyperledger.Aries.Features.IssueCredential;
using IdentifyMe.MVVM;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentifyMe.ViewModels.Credentials
{
    public class CredentialViewModel : BaseNavigationViewModel
    {

        public CredentialViewModel()
        {

        }

        private CredentialRecord _credentialRecord;

        public CredentialRecord CredentialRecord
        {
            get => _credentialRecord;
            set 
            { 
                RaiseAndUpdate(ref _credentialRecord, value);
                _credId = _credentialRecord.Id;
                _state = _credentialRecord.State;
                
            }
        }

        private string _credId;

        public string Id
        {
            get => _credId;
            set => RaiseAndUpdate(ref _credId, value);
        }

        private CredentialState _state;

        public CredentialState State
        {
            get => _state;
            set => RaiseAndUpdate(ref _state, value);
        }
    }


}
