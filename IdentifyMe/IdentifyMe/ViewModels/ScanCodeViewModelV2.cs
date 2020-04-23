using Acr.UserDialogs;
using IdentifyMe.Framework.Utilities;
using IdentifyMe.Services.Interfaces;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IdentifyMe.ViewModels
{
    public class ScanCodeViewModelV2 : ABaseViewModel
    {

        public ScanCodeViewModelV2(IUserDialogs userDialogs,
            INavigationServiceV2 navigationService) : 
            base("Scan QR", userDialogs, navigationService)
        {
        }


        public ZXing.Result Result { get; set; }

        private bool _isScanning = true;

        public bool IsScanning
        {
            get => _isScanning;
            set => this.RaiseAndSetIfChanged(ref _isScanning, value);
        }

        private bool _isAnalyzing = true;
        public bool IsAnalyzing
        {
            get => _isAnalyzing;
            set => this.RaiseAndSetIfChanged(ref _isAnalyzing, value);
        }

        public async Task ProcessCode()
        {
            string scannedCode = Result.Text;
            //bool comingFromExternalSource = false;
            try
            {
                var message = await MessageDecorder.ParseMessageAsync(scannedCode);
                Console.WriteLine($@"Decoded message {message}");
                await NavigationService.NavigateBackAsync();
                //AcceptInvitationViewModel acceptInvitationViewModel = MakeVm<AcceptInvitationViewModel>();
                //acceptInvitationViewModel.InvitationMessage = (ConnectionInvitationMessage)message;
               // await Navigation.PushPopupAsync<AcceptInvitationViewModel>(acceptInvitationViewModel);
                await Application.Current.MainPage.DisplayAlert("Scanned code", scannedCode, "Close");
                
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await NavigationService.NavigateBackAsync();
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid QR Code", "Close");
            }
        }

        public ICommand QRScanResultCommand => new Command(() => {
                
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (IsScanning)
                    {
                        IsAnalyzing = false;
                        IsScanning = false;
                        await this.ProcessCode();
                    }
                    //do your job here - Result.Text contains QR CODE
                    IsAnalyzing = true;
                    IsScanning = true;
                });
              
        });

    }
}
