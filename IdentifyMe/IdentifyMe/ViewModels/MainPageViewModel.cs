using IdentifyMe.MVVM;

namespace IdentifyMe.ViewModels
{
    public class MainPageViewModel : BaseNavigationViewModel
    {
        public HomePageViewModel Home { get; set; }

        public MainPageViewModel(HomePageViewModel homePageViewModel)
        {
            Home = homePageViewModel;
        }
    }
}