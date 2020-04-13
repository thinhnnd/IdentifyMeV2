using IdentifyMe.MVVM;
using IdentifyMe.ViewModels.Connections;

namespace IdentifyMe.ViewModels
{
    public class MainPageViewModel : BaseNavigationViewModel
    {
        public HomePageViewModel Home { get; set; }
        public ConnectionsViewModel Connections { get; set; }  
        public MainPageViewModel(HomePageViewModel homePageViewModel, 
            ConnectionsViewModel connectionsViewModel)
        {
            Home = homePageViewModel;
            Connections = connectionsViewModel;
        }
    }
}