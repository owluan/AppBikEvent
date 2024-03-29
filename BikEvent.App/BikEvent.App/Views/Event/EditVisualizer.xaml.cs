using BikEvent.App.Models;
using BikEvent.App.Services;
using BikEvent.Domain.Models;
using Newtonsoft.Json;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.PancakeView;
using Xamarin.Forms.Xaml;

namespace BikEvent.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditVisualizer : ContentPage
    {
        private Event _event { get; set; }
        private Position selectedLocation { get; set; }
        private EventService _eventService;
        private CommentService _commentService;
        private int _currentIndex;

        private ObservableCollection<Comment> comments = new ObservableCollection<Comment>();

        public EditVisualizer(Event eventToShow)
        {
            InitializeComponent();
            _eventService = new EventService();
            _commentService = new CommentService();
            _event = eventToShow;
            BindingContext = _event;
            DeserializeObject();
            ImageCarousel.ItemsSource = null;
            ImageCarousel.ItemsSource = _event.ImageList;
            selectedLocation = new Position(_event.Latitude, _event.Longitude);
            UpdateMapView(selectedLocation);
            LoadComments();
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

                    await Navigation.PopAsync();
                }
                else
                {
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

            if (ImageCarousel.ItemsSource == null)
            {
                ImageLayout.IsVisible = false;
            }

            if (ImageCarousel.ItemsSource != null && _event.ImageList.Count < 1)
            {
                ImageLayout.IsVisible = false;
            }

            if (ImageCarousel.ItemsSource != null && _event.ImageList.Count < 2)
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

            totalHeight += (comments.Count - 1) * 50;

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

        private async void LoadComments()
        {
            ResponseService<List<Comment>> responseService = await _commentService.GetComments(_event.Id);

            if (responseService.IsSuccess)
            {
                comments = new ObservableCollection<Comment>(responseService.Data);
                TxtCommentsCount.Text = $"{comments.Count()} comentário(s).";
                CommentsListView.IsVisible = true;
            }
            else
            {
                await DisplayAlert("Erro", "Oops! Ocorreu um erro inesperado, tente novamente mais tarde.", "OK");
                CommentsListView.IsVisible = false;
            }

            CommentsListView.ItemsSource = comments;

            UpdateListViewHeight();
        }

        private async void CommentClicked(object sender, EventArgs e)
        {
            User user = JsonConvert.DeserializeObject<User>((string)App.Current.Properties["User"]);

            if (!string.IsNullOrWhiteSpace(CommentEntry.Text))
            {
                Comment newComment = new Comment
                {
                    UserName = user.Name,
                    CommentText = CommentEntry.Text,
                    UserId = user.Id,
                    EventId = _event.Id
                };

                ResponseService<Comment> responseService = await _commentService.AddComment(newComment);

                if (responseService.IsSuccess)
                {
                    comments.Add(responseService.Data);

                    TxtCommentsCount.Text = $"{comments.Count()} comentário(s).";

                    LoadComments();

                    CommentEntry.Text = string.Empty;
                    CommentEntry.Unfocus();

                    ScrollToLastComment();
                }
                else
                {
                    await DisplayAlert("Erro", "Não foi possível salvar o comentário. Por favor, tente novamente mais tarde.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Erro", "Por favor, digite um comentário antes de enviar.", "OK");
            }
        }

        private async void DeleteCommentClicked(object sender, EventArgs e)
        {
            if (sender is ImageButton deleteButton && deleteButton.CommandParameter is int commentIdToDelete)
            {
                bool userConfirmed = await DisplayAlert("Confirmação", "Tem certeza de que deseja excluir este comentário?", "Sim", "Não");

                if (userConfirmed)
                {
                    ResponseService<Comment> responseService = await _commentService.DeleteComment(commentIdToDelete);

                    if (responseService.IsSuccess)
                    {
                        LoadComments();
                    }
                    else
                    {
                        await DisplayAlert("Erro", "Ocorreu um erro ao excluir o comentário.", "OK");
                    }
                }
            }
        }

        private void ScrollToLastComment()
        {
            if (comments.Any())
            {
                var lastComment = comments.Last();

                int lastIndex = comments.IndexOf(lastComment);

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Task.Delay(100);
                    CommentsListView.ScrollTo(lastIndex, 0, ScrollToPosition.End, true);
                });
            }
        }

        private void PancakeView_BindingContextChanged(object sender, EventArgs e)
        {
            User user = JsonConvert.DeserializeObject<User>((string)App.Current.Properties["User"]);
            var pancakeView = sender as PancakeView;
            if (pancakeView != null)
            {
                var comment = pancakeView.BindingContext as Comment;
                if (comment != null)
                {
                    if (comment.UserId == user.Id) 
                    {
                        var deleteButton = pancakeView.FindByName<ImageButton>("DeleteCommentButton");
                        if (deleteButton != null)
                        {
                            deleteButton.IsVisible = true;
                        }
                    }
                    else
                    {
                        var deleteButton = pancakeView.FindByName<ImageButton>("DeleteCommentButton");
                        if (deleteButton != null)
                        {
                            deleteButton.IsVisible = false;
                        }
                    }
                }
            }
        }
    }
}