using Acr.UserDialogs;
using IdentifyMe.Framework.Services;
using IdentifyMe.Messages;
using IdentifyMe.Services.Interfaces;
using IdentifyMe.ViewModels.Connections;
using IdentifyMe.ViewModels.Credentials;
using IdentifyMe.ViewModels.Notification;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IdentifyMe.ViewModels
{
    public class MainViewModel : ABaseViewModel
    {
        private CloudWalletService _cloudWalletService;

        public MainViewModel(
             IUserDialogs userDialogs,
             INavigationServiceV2 navigationService,
             ConnectionsViewModelV2 connectionsViewModel,
             CredentialsViewModelV2 credentialsViewModel,
             NotificationViewModelV2 notificationViewModel,
             CloudWalletService cloudWalletService
         ) : base(
                 nameof(MainViewModel),
                 userDialogs,
                 navigationService
         )
        {
            Connections = connectionsViewModel;
            Credentials = credentialsViewModel;
            Notification = notificationViewModel;
            _cloudWalletService = cloudWalletService;

        }

        public override async Task InitializeAsync(object navigationData)
        {
            await Connections.InitializeAsync(null);
            await Credentials.InitializeAsync(null);
            await Notification.InitializeAsync(null);
            await base.InitializeAsync(navigationData);
            MessagingCenter.Send(new StartLongRunningTaskMessage(), "StartLongRunningTaskMessage");
            HandleReceivedMessages();
        }

        private void HandleReceivedMessages()
        {
            System.Diagnostics.Debug.WriteLine($"Tikcked Messsage 1: work");
            MessagingCenter.Unsubscribe<TickedMessage >(this, "TickedMessage"); 

            MessagingCenter.Subscribe<TickedMessage>(this, "TickedMessage", message => {

                Device.BeginInvokeOnMainThread(async () => {
                    System.Diagnostics.Debug.WriteLine($"Tikcked Messsage 2: { message.Message}");
                    await _cloudWalletService.FetchCloudMessagesAsync();              
                });
            });

            MessagingCenter.Subscribe<CancelledMessage>(this, "CancelledMessage", message =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    //ticker.Text = "Cancelled";
                });
            });
        }

        #region Bindable Properties
        private ConnectionsViewModelV2 _connections;
        public ConnectionsViewModelV2 Connections
        {
            get => _connections;
            set => this.RaiseAndSetIfChanged(ref _connections, value);
        }

        private CredentialsViewModelV2 _credentials;
        public CredentialsViewModelV2 Credentials
        {
            get => _credentials;
            set => this.RaiseAndSetIfChanged(ref _credentials, value);
        }

        private NotificationViewModelV2 _notification;
        public NotificationViewModelV2 Notification
        {
            get => _notification;
            set => this.RaiseAndSetIfChanged(ref _notification, value);
        }

        //private SettingViewModelV2 _setting;
        //public SettingViewModelV2 CreateInvitation
        //{
        //    get => _createInvitation;
        //    set => this.RaiseAndSetIfChanged(ref _createInvitation, value);
        //}
        #endregion
    }
}
