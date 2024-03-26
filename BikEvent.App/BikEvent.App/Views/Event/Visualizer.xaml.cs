using BikEvent.App.Models;
using BikEvent.App.Resources.Controls;
using BikEvent.App.Services;
using BikEvent.Domain.Models;
using FFImageLoading;
using Newtonsoft.Json;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace BikEvent.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Visualizer : ContentPage
    {
        private Event _event { get; set; }
        private Position selectedLocation { get; set; }
        private EventService _eventService;
        private int _currentIndex;

        List<Comment> comments = new List<Comment>
        {
            new Comment { UserName = "Usuário1", CommentText = "Este é o 1 comentário.Este é o primeiro comentário." },
            new Comment { UserName = "Usuário1", CommentText = "Este é o 1 comentário.Este é o primeiro comentário." },
            new Comment { UserName = "Usuário1", CommentText = "Este é o 1 comentário.Este é o primeiro comentário." },
            new Comment { UserName = "Usuário1", CommentText = "Este é o 1 comentário.Este é o primeiro comentário." },
        };

        public Visualizer(Event eventToShow)
        {
            InitializeComponent();
            _eventService = new EventService();
            _event = eventToShow;
            BindingContext = _event;
            DeserializeObject();
            ImageCarousel.ItemsSource = null;
            ImageCarousel.ItemsSource = _event.ImageList;
            selectedLocation = new Position(_event.Latitude, _event.Longitude);
            UpdateMapView(selectedLocation);
            CommentsListView.ItemsSource = comments;
            TxtCommentsCount.Text = $"{comments.Count()} comentário(s).";
            UpdateListViewHeight();
            HideFields();
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

            if (_event.ImageList.Count < 1)
            {
                ImageLayout.IsVisible = false;
            }

            if (_event.ImageList.Count < 2)
            {
                ArrowButton.IsVisible = false;
            }

            if (_event.Latitude == 0 && _event.Longitude == 0)
            {
                HowToGetButton.IsVisible = false;
            }

            if (selectedLocation.Latitude == 0 && selectedLocation.Longitude == 0)
            {
                MapLayout.IsVisible = false;
            }
            else { MapLayout.IsVisible = true; }

            if (comments.Count > 0)
            {
                CommentsListView.IsVisible = true;
            }
            else
            {
                CommentsListView.IsVisible = false;
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

        private void OnPreviousButtonClicked(object sender, EventArgs e)
        {
            _currentIndex = (_currentIndex - 1 + _event.ImageList.Count) % _event.ImageList.Count;

            ImageCarousel.Position = _currentIndex;
        }

        private void OnNextButtonClicked(object sender, EventArgs e)
        {
            _currentIndex = (_currentIndex + 1) % _event.ImageList.Count;

            ImageCarousel.Position = _currentIndex;
        }

        private async void OnHowToGetClicked(object sender, EventArgs e)
        {
            await Xamarin.Essentials.Map.OpenAsync(_event.Latitude, _event.Longitude, new MapLaunchOptions { Name = "Como chegar", NavigationMode = NavigationMode.Bicycling });
        }

        private void UpdateMapView(Position location)
        {
            EventMap.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(1)));

            var pin = new Pin
            {
                Position = location,
                Label = "Local do Evento",
                Type = PinType.SavedPin
            };

            EventMap.Pins.Clear();
            EventMap.Pins.Add(pin);
        }

        private void UpdateListViewHeight()
        {
            double totalHeight = 0;
            double minHeight = 250;

            foreach (var comment in comments)
            {
                var textSize = MeasureTextSize(comment.CommentText);
                double labelHeight = textSize.Height;
                totalHeight += labelHeight;
            }

            // Adicione espaço adicional, se necessário
            totalHeight += (comments.Count - 1) * 50; // Adicionando espaço entre os itens

            // Definir a altura da CollectionView
            CommentsListView.HeightRequest = Math.Max(totalHeight, minHeight);
        }

        private SKSize MeasureTextSize(string text)
        {
            var paint = new SKPaint
            {
                TextSize = (float)Device.GetNamedSize(NamedSize.Caption, typeof(Editor)),
                IsAntialias = true
            };
            SKRect bounds = new SKRect();
            paint.MeasureText(text, ref bounds);
            return new SKSize(bounds.Width, bounds.Height);
        }
    }
}