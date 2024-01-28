using BikEvent.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BikEvent.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditVisualizer : ContentPage
    {
        private Event _event { get; set; }

        public EditVisualizer(Event eventToShow)
        {
            InitializeComponent();
            _event = eventToShow;
            BindingContext = _event;
            HideFields();
        }

        private async void GoBack(object sender, EventArgs e)
        {
            await GoBackAsync();
        }

        private async Task GoBackAsync()
        {
            var navigationStack = Navigation.NavigationStack.ToList();
            if (navigationStack.Count >= 2)
            {
                Navigation.RemovePage(navigationStack[1]);
            }

            await Navigation.PushAsync(new MyEvents());
        }       

        private async void EditEvent(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditEvent(_event));
        }

        private void HideFields()
        {
            User user = JsonConvert.DeserializeObject<User>((string)App.Current.Properties["User"]);

            if (_event.UserId != user.Id)
            {
                EditButton.IsVisible = false;
            }

            if (string.IsNullOrEmpty(_event.SocialMedia))
            {
                HeaderSocialMedia.IsVisible = false;
                TextSocialMedia.IsVisible = false;
            }

            if (string.IsNullOrEmpty(_event.Description))
            {
                HeaderDescription.IsVisible = false;
                TextDescription.IsVisible = false;
            }

            if (string.IsNullOrEmpty(_event.Benefits))
            {
                HeaderBenefits.IsVisible = false;
                TextBenefits.IsVisible = false;
            }
        }
    }
}