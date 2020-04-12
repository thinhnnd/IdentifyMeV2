using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Hyperledger.Aries.Agents.Edge;
using Hyperledger.Aries.Configuration;
using Microsoft.Extensions.Options;
using IdentifyMe.Configuration;
using IdentifyMe.Framework.Services;
using IdentifyMe.MVVM;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace IdentifyMe.ViewModels
{
    public class HomePageViewModel : BaseNavigationViewModel, INavigationAware
    {
        private readonly IEdgeProvisioningService _edgeProvisioningService;
        private readonly IWalletAppConfiguration _walletConfiguration;
        private readonly AgentOptions _options;
        private CloudWalletService _cloudWalletService;

        public ICommand CreateWalletCommand { get; }
        public ICommand GoToScanCommand { get; }

        public ICommand FetchInboxCommand { get; }

        public HomePageViewModel(IEdgeProvisioningService edgeProvisioningService, IWalletAppConfiguration walletconfiguration, IOptions<AgentOptions> options,CloudWalletService cloudWalletService)
        {
            _edgeProvisioningService = edgeProvisioningService;
            _walletConfiguration = walletconfiguration;
            _options = options.Value;
            _cloudWalletService = cloudWalletService;
            CreateWalletCommand = new AsyncCommand(CreateAgent);
            GoToScanCommand = new AsyncCommand(GoToScan);
            FetchInboxCommand = new AsyncCommand(FetchInbox);
        }

        public void OnNavigatedTo()
        {
            Console.WriteLine(@"Just got here");
        }

        public void OnNavigatingFrom()
        {
            Console.WriteLine(@"Navigating Out");
        }

        public void OnNavigatingTo()
        {
            Console.WriteLine(@"Navigating Out");
        }

        private async Task CreateAgent()
        {
            Preferences.Set(Constants.PoolConfigurationName, _walletConfiguration.PoolConfigurationName);
            IsBusy = true;
            try
            {
                _options.AgentName = DeviceInfo.Name;
                _options.WalletConfiguration.Id = Constants.LocalWalletIdKey;
                _options.WalletCredentials.Key = await SyncedSecureStorage.GetOrCreateSecureAsync(
                    key: Constants.LocalWalletCredentialKey,
                    value: Utils.Utils.GenerateRandomAsync(32));
                await _edgeProvisioningService.ProvisionAsync(_options);
                await Application.Current.MainPage.DisplayAlert("Wallet created", "", "Ok");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, "Wasn't able to provision the agent");
                await Application.Current.MainPage.DisplayAlert("An error occured while registering the wallet", "", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
        
        private async Task GoToScan()
        {
            await Navigation.PushModalAsync(MakeVm<ScanCodeViewModel>());
        }

        private async Task FetchInbox()
        {
            await _cloudWalletService.FetchCloudMessagesAsync();
        }
    }
}
