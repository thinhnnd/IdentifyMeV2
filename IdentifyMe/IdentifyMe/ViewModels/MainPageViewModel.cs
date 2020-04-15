using IdentifyMe.MVVM;
using IdentifyMe.ViewModels.Connections;
using IdentifyMe.ViewModels.Notification;
using System.Reactive;

namespace IdentifyMe.ViewModels
{
    public class MainPageViewModel : BaseNavigationViewModel
    {
        public HomePageViewModel Home { get; set; }
        public ConnectionsViewModel Connections { get; set; }  

        public NotificationViewModel Notification { get; set; }
        public MainPageViewModel(HomePageViewModel homePageViewModel, 
            NotificationViewModel notificationViewModel,
            ConnectionsViewModel connectionsViewModel)
        {
            Home = homePageViewModel;
            Connections = connectionsViewModel;
            Notification = notificationViewModel;
        }
    }
}