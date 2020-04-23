using Acr.UserDialogs;
using IdentifyMe.Services.Interfaces;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentifyMe.ViewModels
{
    class TestViewModel : ABaseViewModel
    {
        public TestViewModel(IUserDialogs userDialogs,
            INavigationServiceV2 navigationService) : base(
                nameof(TestViewModel),
                userDialogs,
                navigationService)
        {

        }

        string _testString = "Yay, It's worked";

        public string TestString
        {
            get => _testString;
            set => this.RaiseAndSetIfChanged(ref _testString, value);
        }
    }
}
