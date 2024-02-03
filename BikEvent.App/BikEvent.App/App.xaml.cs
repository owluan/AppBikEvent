using BikEvent.App.Views;
using Xamarin.Forms;

namespace BikEvent.App
{
    public partial class App : Application
    {
        public App()
        {   
            InitializeComponent();
            CheckUserAndNavigateAsync();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private void CheckUserAndNavigateAsync()
        {
            if (App.Current.Properties.ContainsKey("User"))
            {
                var menuPage = new MenuPage();
                menuPage.Title = "Navigation";

                var mainPage = new MasterDetailPage
                {
                    Master = menuPage,
                    Detail = new NavigationPage(new Initial()) // Set MainPage as the detail page
                };

                // Set MainPage as the new root page
                App.Current.MainPage = mainPage;
            }
            else
            {
                App.Current.MainPage = new NavigationPage(new Login());
            }
        }

    }
}
