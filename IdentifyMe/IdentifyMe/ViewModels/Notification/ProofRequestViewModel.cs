using Hyperledger.Aries.Features.PresentProof;
using IdentifyMe.MVVM;
using Newtonsoft.Json.Linq;
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
            Console.WriteLine("On Navigated To");
        }

        public void OnNavigatingFrom()
        {
            Console.WriteLine("On Navigating From");
        }

        public void OnNavigatingTo()
        {
            Console.WriteLine("On Navigating To");
        }

        #region Bindable Props 
        private ProofRecord _proofRequestRecord;

        public ProofRecord ProofRequestRecord
        {
            get => _proofRequestRecord;
            set 
            {
                RaiseAndUpdate(ref _proofRequestRecord, value);
                _requestedProof = JObject.Parse(_proofRequestRecord.RequestJson);
            }
        }

        private JObject _requestedProof;

        public JObject RequestedProof
        {
            get => _requestedProof;
            set
            {
                RaiseAndUpdate(ref _requestedProof, value);
            }
        }


        #endregion

        #region Bindable Command 
        public ICommand RejectProofRequestCommand => new Command( async () => await Application.Current.MainPage.DisplayAlert("Receject Proof Request","", "OK"));
        public ICommand AcceptProofRequestCommand => new Command(async () => await Application.Current.MainPage.DisplayAlert("Accept Proof Request", "", "Ok"));
        #endregion
    }
}
