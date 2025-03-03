using BikEvent.App.Models;
using BikEvent.App.Resources.Load;
using BikEvent.App.Services;
using BikEvent.Domain.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BikEvent.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register : ContentPage
    {
        private UserService _userService;

        public Register()
        {
            InitializeComponent();
            _userService = new UserService();
        }

        private void GoBack(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void SaveAction(object sender, EventArgs e)
        {
            TxtMessages.Text = String.Empty;

            User user = new User()
            {
                Name = TxtName.Text,
                Email = TxtEmail.Text,
                Password = TxtPassword.Text,
                CityState = TxtCityState.Text,
                ProfilePhoto = "cyclistmale.png",
                CoverPhoto = "coverphotodefault.jpeg"
            };

            await Navigation.PushPopupAsync(new Loading());

            ResponseService<User> responseService = await _userService.AddUser(user);

            if (responseService.IsSuccess)
            {
                App.Current.Properties.Add("User", JsonConvert.SerializeObject(responseService.Data));
                await App.Current.SavePropertiesAsync();

                var menuPage = new MenuPage();
                menuPage.Title = "Navigation";

                var mainPage = new FlyoutPage
                {
                    Flyout = menuPage,
                    Detail = new NavigationPage(new Initial())
                };

                App.Current.MainPage = mainPage;
            }
            else
            {
                if (responseService.StatusCode == 400)
                {
                    StringBuilder stringBuilder = new StringBuilder();

                    foreach (var itemKey in responseService.Errors)
                    {
                        foreach (var message in itemKey.Value)
                        {
                            stringBuilder.AppendLine(message);
                        }
                    }
                    TxtMessages.Text = stringBuilder.ToString();
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