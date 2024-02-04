using BikEvent.App.Models;
using BikEvent.App.Services;
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
    public partial class Visualizer : ContentPage
    {
        private Event _event { get; set; }
        private EventService _eventService;

        public Visualizer(Event eventToShow)
        {
            InitializeComponent();
            _eventService = new EventService();
            _event = eventToShow;
            BindingContext = _event;
            HideFields();
            DeserializeObject();
            // Atualize a lista de imagens quando a página aparecer
            ImageCarousel.ItemsSource = null; // Limpe a fonte existente
            ImageCarousel.ItemsSource = _event.ImageList; // Defina a nova fonte
        }

        private async void GoBack(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
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
                DeleteButton.IsVisible = false;
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

        private async void DeleteEvent(object sender, EventArgs e)
        {
            bool userConfirmed = await DisplayAlert("Confirmação", "Tem certeza de que deseja excluir este evento?", "Sim", "Não");

            if (userConfirmed)
            {               
                ResponseService<Event> responseService = await _eventService.DeleteEvent(_event.Id);

                if (responseService.IsSuccess)
                {
                    await DisplayAlert("Exclusão de Evento", "Evento excluído com sucesso!", "OK");

                    var navigationStack = Navigation.NavigationStack.ToList();
                    if (navigationStack.Count >= 2)
                    {
                        Navigation.RemovePage(navigationStack[1]);
                    }

                    await Navigation.PushAsync(new MyEvents());
                }
                else
                {
                    // Tratar erro, se necessário
                    await DisplayAlert("Erro", "Ocorreu um erro ao excluir o evento.", "OK");
                }
            }
        }

        private void DeserializeObject()
        {
            if (_event.ImageUrl != null)
            {
                _event.ImageList = JsonConvert.DeserializeObject<List<string>>(_event.ImageUrl);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

           
        }
    }
}