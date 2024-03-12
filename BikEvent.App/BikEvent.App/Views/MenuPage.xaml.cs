using BikEvent.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BikEvent.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        public User User { get; set; }
        public ObservableCollection<Domain.Models.MenuItem> MenuItems { get; set; }

        public MenuPage()
        {
            InitializeComponent();

            User = JsonConvert.DeserializeObject<User>((string)App.Current.Properties["User"]);

            menuPageListView = this.FindByName<ListView>("menuPageListView");

            MenuItems = new ObservableCollection<Domain.Models.MenuItem>
            {
                new Domain.Models.MenuItem { Title = "Inicio", TargetType = typeof(Initial) },
                new Domain.Models.MenuItem { Title = "Meus Eventos", TargetType = typeof(MyEvents) },
                new Domain.Models.MenuItem { Title = "Navegação", TargetType = typeof(MapNavigator) },
                new Domain.Models.MenuItem { Title = "Home", TargetType = typeof(Home) }
            };

            BindingContext = this;
        }

        private async void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var selectedItem = e.SelectedItem as Domain.Models.MenuItem;

            if (selectedItem != null)
            {
                Type pageType = selectedItem.TargetType;
                Page page = (Page)Activator.CreateInstance(pageType);

                if (Application.Current.MainPage is MasterDetailPage mainPage)
                {
                    await mainPage.Detail.FadeTo(0, 900);
                    mainPage.Detail = new NavigationPage(page);
                    mainPage.IsPresented = false;
                    await mainPage.Detail.FadeTo(1, 900); 
                }
            }

            ((ListView)sender).SelectedItem = null;
        }

        private void Logout(object sender, EventArgs e)
        {
            App.Current.Properties.Remove("User");
            App.Current.SavePropertiesAsync();
            App.Current.MainPage = new NavigationPage(new Login());
        }

    }
}