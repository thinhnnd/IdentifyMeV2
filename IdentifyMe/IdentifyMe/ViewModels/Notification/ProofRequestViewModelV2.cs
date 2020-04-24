using Acr.UserDialogs;
using Hyperledger.Aries.Features.PresentProof;
using IdentifyMe.Services.Interfaces;
using Newtonsoft.Json.Linq;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IdentifyMe.ViewModels.Notification
{
    public class ProofRequestViewModelV2 : ABaseViewModel
    {
        public ProofRequestViewModelV2(IUserDialogs userDialogs,
            INavigationServiceV2 navigationService) : base (nameof(ProofRequestViewModelV2), userDialogs, navigationService)
        {
            Title = "Proof Request";
        }

        #region Bindable Props 
        private ProofRecord _proofRequestRecord;

        public ProofRecord ProofRequestRecord
        {
            get => _proofRequestRecord;
            set
            {
                this.RaiseAndSetIfChanged(ref _proofRequestRecord, value);
                _requestedProof = JObject.Parse(_proofRequestRecord.RequestJson);
            }
        }

        private JObject _requestedProof;

        public JObject RequestedProof
        {
            get => _requestedProof;
            set
            {
                this.RaiseAndSetIfChanged(ref _requestedProof, value);
            }
        }


        #endregion

        #region Bindable Command 
        public ICommand RejectProofRequestCommand => new Command(async () => await Application.Current.MainPage.DisplayAlert("Receject Proof Request", "", "OK"));
        public ICommand AcceptProofRequestCommand => new Command(async () => await Application.Current.MainPage.DisplayAlert("Accept Proof Request", "", "Ok"));
        #endregion
    }
}
