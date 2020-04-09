using System;
using System.Diagnostics;
using System.Threading.Tasks;
using IdentifyMe.Framework.Utilities;
using IdentifyMe.MVVM;
using Xamarin.Forms;

namespace IdentifyMe.ViewModels
{
    public class ScanCodeViewModel : BaseNavigationViewModel, INavigationAware
    {
        private bool _isScanning;

        public bool IsScanning
        {
            get => _isScanning;
            set => RaiseAndUpdate(ref _isScanning, value);
        }
        
        public async Task ProcessCode(string scannedCode, bool comingFromExternalSource = false)
        {
            try
            {
                IsScanning = false;
                
                var message = await MessageDecorder.ParseMessageAsync(scannedCode);
                Console.WriteLine($@"Decoded message {message}");
                await Application.Current.MainPage.DisplayAlert("Scanned code", scannedCode, "Close");
            }
            catch (Exception e)
            {
                IsScanning = true;
                Debug.WriteLine(e);
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid QR Code", "Close");
            }
        }

        public void OnNavigatingFrom()
        {
            IsScanning = false;
        }

        public void OnNavigatingTo()
        {
            IsScanning = true;
        }

        public void OnNavigatedTo()
        {
        }
    }
}
