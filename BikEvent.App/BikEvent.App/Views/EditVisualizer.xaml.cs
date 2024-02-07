using BikEvent.App.Models;
using BikEvent.App.Services;
using BikEvent.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private EventService _eventService;
        private int _currentIndex;

        public EditVisualizer(Event eventToShow)
        {
            InitializeComponent();
            _eventService = new EventService();
            _event = eventToShow;
            BindingContext = _event;
            DeserializeObject();
            ImageCarousel.ItemsSource = null;
            ImageCarousel.ItemsSource = _event.ImageList;
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

        private async void DeleteEvent(object sender, EventArgs e)
        {
            bool userConfirmed = await DisplayAlert("Confirmação", "Tem certeza de que deseja excluir este evento?", "Sim", "Não");

            if (userConfirmed)
            {
                ResponseService<Event> responseService = await _eventService.DeleteEvent(_event.Id);

                if (responseService.IsSuccess)
                {
                    await DisplayAlert("Exclusão de Evento", "Evento excluído com sucesso!", "OK");

                    // Volte para a página anterior após excluir
                    await Navigation.PopAsync();
                }
                else
                {
                    // Tratar erro, se necessário
                    await DisplayAlert("Erro", "Ocorreu um erro ao excluir o evento.", "OK");
                }
            }
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

            if (_event.ImageList.Count < 2)
            {
                ArrowButton.IsVisible = false;
            }
        }

        private void DeserializeObject()
        {
            if (_event.ImageUrl != null)
            {
                _event.ImageList = JsonConvert.DeserializeObject<List<string>>(_event.ImageUrl);
            }
        }

        private void OnPreviousButtonClicked(object sender, EventArgs e)
        {
            // Navegue para a imagem anterior
            _currentIndex = (_currentIndex - 1 + _event.ImageList.Count) % _event.ImageList.Count;

            // Atualize a posição atual do CarouselView
            ImageCarousel.Position = _currentIndex;
        }

        private void OnNextButtonClicked(object sender, EventArgs e)
        {
            // Navegue para a próxima imagem
            _currentIndex = (_currentIndex + 1) % _event.ImageList.Count;

            // Atualize a posição atual do CarouselView
            ImageCarousel.Position = _currentIndex;
        }
    }
}