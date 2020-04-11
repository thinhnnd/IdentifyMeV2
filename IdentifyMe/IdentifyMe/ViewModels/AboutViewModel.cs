using IdentifyMe.MVVM;

namespace IdentifyMe.ViewModels
{
    public class AboutViewModel : BaseNavigationViewModel
    {
        private string _name = "Test about page";

        public string Name
        {
            get => _name;
            set => RaiseAndUpdate(ref _name, value);
        }
    }
}
