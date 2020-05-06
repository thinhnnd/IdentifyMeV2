using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IdentifyMe.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainNavigationBar : Grid
    {
        public MainNavigationBar()
        {
            InitializeComponent();
        }
    }
}