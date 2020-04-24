using IdentifyMe.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IdentifyMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageV2 : TabbedPage, IRootView
    {
        public MainPageV2()
        {
            InitializeComponent();
        }


        private void CurrentPageChanged(object sender, System.EventArgs e) => Title = GetPageName(CurrentPage);

        private void Appearing(object sender, System.EventArgs e) => Title = GetPageName(CurrentPage);

        private string GetPageName(Page page)
        {
            if (page.BindingContext is ABaseViewModel vmBase)
                return vmBase.Title;
            return null;
        }
    }
}