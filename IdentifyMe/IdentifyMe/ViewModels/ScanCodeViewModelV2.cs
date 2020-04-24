using Acr.UserDialogs;
using Autofac;
using Hyperledger.Aries.Features.DidExchange;
using IdentifyMe.Framework.Utilities;
using IdentifyMe.Services.Interfaces;
using IdentifyMe.ViewModels.Connections;
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
       private readonly ILifetimeScope _scope;
        public ScanCodeViewModelV2(IUserDialogs userDialogs,
            INavigationServiceV2 navigationService,
            ILifetimeScope scope) : 
            base(nameof(ScanCodeViewModelV2), userDialogs, navigationService)
        {
            _scope = scope;
            Title = "Scan QR";
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
                AcceptInvitationViewModelV2 acceptInvitationViewModel = _scope.Resolve<AcceptInvitationViewModelV2>();
                acceptInvitationViewModel.InvitationMessage = (ConnectionInvitationMessage)message;
                await NavigationService.NavigateBackAsync();
                await NavigationService.NavigateToPopupAsync<AcceptInvitationViewModelV2>(true, acceptInvitationViewModel);
                //await Application.Current.MainPage.DisplayAlert("Scanned code", scannedCode, "Close");
                
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
