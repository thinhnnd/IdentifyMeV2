using Hyperledger.Aries.Features.PresentProof;
using IdentifyMe.MVVM;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IdentifyMe.ViewModels.Notification
{
    public class ProofRequestViewModel : BaseNavigationViewModel, INavigationAware 
    {
        public ProofRequestViewModel()
        {
            Title = "Proof Request";

        }

        public void OnNavigatedTo()
        {
            throw new NotImplementedException();
        }

        public void OnNavigatingFrom()
        {
            throw new NotImplementedException();
        }

        public void OnNavigatingTo()
        {
            throw new NotImplementedException();
        }

        #region Bindable Props 
        private ProofRecord _proofRequestRecord;

        public ProofRecord ProofRequestRecord
        {
            get => _proofRequestRecord;
            set => RaiseAndUpdate(ref _proofRequestRecord, value);
        }
        #endregion

        #region Bindable Command 
        public ICommand RejectProofRequestCommand => new Command( async () => await Application.Current.MainPage.DisplayAlert("Receject Proof Request","", "OK"));
        public ICommand AcceptProofRequestCommand => new Command(async () => await Application.Current.MainPage.DisplayAlert("Accept Proof Request", "", "Ok"));
        #endregion
    }
}
