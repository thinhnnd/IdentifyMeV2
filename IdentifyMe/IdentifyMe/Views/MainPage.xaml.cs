using System;
using System.ComponentModel;
using IdentifyMe.MVVM;
using IdentifyMe.ViewModels;
using Xamarin.Forms;

namespace IdentifyMe.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage 
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void CurrentPageChanged(object sender, System.EventArgs e) => Title = GetPageName(CurrentPage);

        private void Appearing(object sender, System.EventArgs e) => Title = GetPageName(CurrentPage);

        private string GetPageName(Page page)
        {
            if (page.BindingContext is BaseNavigationViewModel vmBase)
                return vmBase.Title;
            return null;
        }
    }
}