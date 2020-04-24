using IdentifyMe.Framework.Services;
using IdentifyMe.Messages;
using IdentifyMe.MVVM;
using IdentifyMe.ViewModels.Connections;
using IdentifyMe.ViewModels.Credentials;
using IdentifyMe.ViewModels.Notification;
using System.Reactive;
using Xamarin.Forms;

namespace IdentifyMe.ViewModels
{
    public class MainPageViewModel : BaseNavigationViewModel
    {
        public HomePageViewModel Home { get; set; }
        public ConnectionsViewModel Connections { get; set; }  

        public NotificationViewModel Notification { get; set; }

        public CredentialsViewModel Credentials { get; set; }

        private CloudWalletService _cloudWalletService;

        public MainPageViewModel(HomePageViewModel homePageViewModel, 
            NotificationViewModel notificationViewModel,
            CredentialsViewModel credentialsViewModel,
            ConnectionsViewModel connectionsViewModel, 
            CloudWalletService cloudWalletService)
        {
            Home = homePageViewModel;
            Connections = connectionsViewModel;
            Notification = notificationViewModel;
            Credentials = credentialsViewModel;
           
        }

    }
}