using IdentifyMe.Framework.Services;
using IdentifyMe.MVVM;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentifyMe.ViewModels.Connections
{
    public class ConnectionsViewModel : BaseNavigationViewModel, INavigationAware
    {
        ConnectionsViewModel(CloudWalletService cloudWalletService)
        {
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
    }
}
