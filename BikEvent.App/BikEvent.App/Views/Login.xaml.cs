using BikEvent.App.Models;
using BikEvent.App.Services;
using BikEvent.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BikEvent.App.Resources.Load;

namespace BikEvent.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        private UserService _userService;

        public Login()
        {
            InitializeComponent();

            _userService = new UserService();
        }

        private async void GoRegister(object sender, EventArgs e)
        {            
            await Navigation.PushAsync(new Register());
        }

        private async void GoInitial(object sender, EventArgs e)
        {
            string email = TxtEmail.Text;
            string password = TxtPassword.Text;

            await Navigation.PushPopupAsync(new Loading());

            ResponseService<User> responseService = await _userService.GetUser(email, password);

            if (responseService.IsSuccess)
            {
                App.Current.Properties.Add("User", JsonConvert.SerializeObject(responseService.Data));
                await App.Current.SavePropertiesAsync();

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
                if (responseService.StatusCode == 404)
                {
                    await DisplayAlert("Erro", "Usuário ou senha incorretos!", "OK");
                }
                else
                {
                    await DisplayAlert("Erro", "Oops! Ocorreu um erro inesperado, tente novamente mais tarde.", "OK");
                }
            }
            await Navigation.PopAllPopupAsync();
        }
    }
}