using IdentifyMe.MVVM;

namespace IdentifyMe.ViewModels
{
    public class MainPageViewModel : BaseNavigationViewModel
    {
        public HomePageViewModel Home { get; set; }
        public AboutViewModel About { get; set; }

        public MainPageViewModel(
            HomePageViewModel homePageViewModel,
            AboutViewModel aboutViewModel)
        {
            Home = homePageViewModel;
            About = aboutViewModel;
        }
    }
}