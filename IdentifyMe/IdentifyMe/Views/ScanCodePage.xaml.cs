using System;
using System.Collections.Generic;
using Autofac;
using IdentifyMe.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Mobile;

namespace IdentifyMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanCodePage
    {
        public ScanCodePage()
        {
            InitializeComponent();
              
            ScannerView.Options = new MobileBarcodeScanningOptions
            {
                UseNativeScanning = true,
                TryHarder = true,
                AutoRotate = true,
                DisableAutofocus = false,
                PossibleFormats = new List<BarcodeFormat>
                {
                    BarcodeFormat.QR_CODE
                },
            };

            if (DeviceInfo.Platform == DevicePlatform.Android)
                ScannerView.Options.CameraResolutionSelector =
                    App.Container.Resolve<IZXingHelper>().CameraResolutionSelectorDelegateImplementation;
        }
        
        private void OnOnScanResult(Result result)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await ViewModel.ProcessCode(result.Text);
            });
            Console.WriteLine(result.Text);
        }
    }
}