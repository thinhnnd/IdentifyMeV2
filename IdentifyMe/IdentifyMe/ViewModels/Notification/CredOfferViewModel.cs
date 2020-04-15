using Hyperledger.Aries.Features.IssueCredential;
using IdentifyMe.MVVM;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IdentifyMe.ViewModels.Notification
{
    public class CredOfferViewModel : BaseNavigationViewModel, INavigationAware
    {
        public CredOfferViewModel()
        {
            Title = "Credential Offer";
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
        private CredentialRecord _credentialOffer;
        public CredentialRecord CredentialOffer
        {
            get => _credentialOffer;
            set => RaiseAndUpdate(ref _credentialOffer, value);
        }
        #endregion

        #region Bindable Command
        private void RejectOffer ()
        {
        }
        public ICommand RejectCredentialOfferCommand => new Command(async () => await Application.Current.MainPage.DisplayAlert("Receject Credential Offer", "", "OK"));
        public ICommand AcceptCredentialOfferCommand => new Command(async () => await Application.Current.MainPage.DisplayAlert("Accept Credential Offer", "", "Ok"));
        #endregion
    }
}
