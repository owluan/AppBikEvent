using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BikEvent.App.Resources.Load
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Loading : PopupPage
    {
        public Loading()
        {
            InitializeComponent();
        }
    }
}