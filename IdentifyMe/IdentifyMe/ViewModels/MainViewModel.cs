using Acr.UserDialogs;
using IdentifyMe.Services.Interfaces;
using IdentifyMe.ViewModels.Connections;
using IdentifyMe.ViewModels.Credentials;
using IdentifyMe.ViewModels.Notification;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentifyMe.ViewModels
{
    public class MainViewModel : ABaseViewModel
    {
             //ConnectionsViewModel connectionsViewModel,
             //CredentialsViewModel credentialsViewModel,
             //AccountViewModel accountViewModel,
             //CreateInvitationViewModel createInvitationViewModel
        public MainViewModel(
             IUserDialogs userDialogs,
             INavigationServiceV2 navigationService,
             ConnectionsViewModelV2 connectionsViewModel,
             CredentialsViewModelV2 credentialsViewModel,
             NotificationViewModelV2 notificationViewModel
         ) : base(
                 nameof(MainViewModel),
                 userDialogs,
                 navigationService
         )
        {
            Connections = connectionsViewModel;
            Credentials = credentialsViewModel;
            Notification = notificationViewModel;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            await Connections.InitializeAsync(null);
            await Credentials.InitializeAsync(null);
            await base.InitializeAsync(navigationData);
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
