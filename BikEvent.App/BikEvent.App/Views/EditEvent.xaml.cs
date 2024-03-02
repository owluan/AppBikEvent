using BikEvent.App.Models;
using BikEvent.App.Resources.Converters;
using BikEvent.App.Resources.Load;
using BikEvent.App.Services;
using BikEvent.Domain.Models;
using BikEvent.Domain.Utility.Enums;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class EditEvent : ContentPage
    {
        private EventService _eventService;
        private Event _eventToEdit;
        private AzureStorageService _azureStorageService;

        public event EventHandler<MapPageEventArgs> MapPageReady;

        private int _currentIndex;
        private Position selectedLocation { get; set; }

        private List<string> _imageUrl { get; set; }
        private List<ImageSource> _imageSources { get; set; }
        private List<Stream> _tempImageStreams { get; set; }
        private List<ImageSource> _imageSourceList { get; set; }


        public EditEvent(Event eventToEdit)
        {
            InitializeComponent();
            _eventService = new EventService();
            _azureStorageService = new AzureStorageService();
            _eventToEdit = eventToEdit;

            _imageUrl = new List<string>();
            _imageSources = new List<ImageSource>();
            _tempImageStreams = new List<Stream>();
            selectedLocation = new Position(_eventToEdit.Latitude, _eventToEdit.Longitude);
            FillFieldsWithEventData();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _imageSourceList = _eventToEdit.ImageList
                    .Select(imageUrl => (ImageSource)imageUrl)
                    .Concat(_imageSources)
                    .ToList();

            ImageCarousel.ItemsSource = null;
            ImageCarousel.ItemsSource = _imageSourceList;            
            HideFields();
        }

        private void FillFieldsWithEventData()
        {
            TxtEventTitle.Text = _eventToEdit.EventTitle;
            TxtCityState.Text = _eventToEdit.CityState;
            DatePicker.Date = _eventToEdit.NextEventDate;
            SetRadioButtonBasedOnRepeatInterval(_eventToEdit.RepeatInterval);
            SetRadioButtonBasedOnEventType(_eventToEdit.EventType);
            SetRadioButtonBasedOnDifficulty(_eventToEdit.Difficulty);
            TxtTags.Text = _eventToEdit.Tag;
            TxtSocialMedia.Text = _eventToEdit.SocialMedia;
            TxtDescription.Text = _eventToEdit.Description;
            TxtBenefits.Text = _eventToEdit.Benefits;
            TxtPhoneNumber.Text = _eventToEdit.PhoneNumber;
            UpdateMapView(selectedLocation);
        }

        private void GoBack(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void Save(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new Loading());

            await UploadImage(sender, e);

            TxtMessages.Text = String.Empty;

            _eventToEdit.EventTitle = TxtEventTitle.Text;
            _eventToEdit.CityState = TxtCityState.Text;
            _eventToEdit.EventDate = DatePicker.Date;
            _eventToEdit.Tag = TxtTags.Text;
            _eventToEdit.SocialMedia = TxtSocialMedia.Text;
            _eventToEdit.Description = TxtDescription.Text;
            _eventToEdit.Benefits = TxtBenefits.Text;
            _eventToEdit.PhoneNumber = TxtPhoneNumber.Text;
            _eventToEdit.EventType = GetSelectedEventType();
            _eventToEdit.RepeatInterval = GetSelectedRepeatInterval();
            _eventToEdit.Difficulty = GetSelectedDifficulty();
            _eventToEdit.ImageUrl = JsonConvert.SerializeObject(_eventToEdit.ImageList);

            ResponseService<Event> responseService = await _eventService.EditEvent(_eventToEdit);

            if (responseService.IsSuccess)
            {
                var navigationStack = Navigation.NavigationStack.ToList();

                if (navigationStack.Count >= 2)
                {
                    Navigation.RemovePage(navigationStack[1]);
                    Navigation.RemovePage(navigationStack[2]);
                }

                await Navigation.PushAsync(new EditVisualizer(_eventToEdit));
                await Navigation.PopAllPopupAsync();
                await DisplayAlert("Edição de Evento", "Evento editado com sucesso!", "OK");
            }
            else
            {
                // Tratar erros conforme necessário
            }
        }

        private void TxtSocialMedia_OnUnfocused(object sender, FocusEventArgs e)
        {
            ScrollToTop();
        }

        private void ScrollToTop()
        {
            if (MyScrollView != null)
            {
                MyScrollView.ScrollToAsync(0, 0, true);
            }
        }

        private void ScrollToBottom()
        {
            if (MyScrollView != null)
            {
                double scrollHeight = MyScrollView.ContentSize.Height - MyScrollView.Height;
                MyScrollView.ScrollToAsync(0, scrollHeight, true);
            }
        }

        private void SetRadioButtonBasedOnEventType(string eventType)
        {
            RBPedal.IsChecked = false;
            RBTrilha.IsChecked = false;
            RBPasseio.IsChecked = false;
            RBCompeticao.IsChecked = false;
            RBEncontro.IsChecked = false;
            RBOutro.IsChecked = false;

            switch (eventType)
            {
                case "Pedal":
                    RBPedal.IsChecked = true;
                    break;
                case "Trilha":
                    RBTrilha.IsChecked = true;
                    break;
                case "Passeio":
                    RBPasseio.IsChecked = true;
                    break;
                case "Competição":
                    RBCompeticao.IsChecked = true;
                    break;
                case "Encontro":
                    RBEncontro.IsChecked = true;
                    break;
                case "Outro":
                    RBOutro.IsChecked = true;
                    break;
                default:
                    break;
            }
        }

        private string GetSelectedEventType()
        {
            if (RBPedal.IsChecked)
                return "Pedal";
            if (RBTrilha.IsChecked)
                return "Trilha";
            if (RBPasseio.IsChecked)
                return "Passeio";
            if (RBCompeticao.IsChecked)
                return "Competição";
            if (RBEncontro.IsChecked)
                return "Encontro";
            if (RBOutro.IsChecked)
                return "Outro";

            return "Default";
        }

        private void SetRadioButtonBasedOnRepeatInterval(RepeatInterval repeatInterval)
        {
            RBNone.IsChecked = false;
            RBWeekly.IsChecked = false;
            RBBiWeekly.IsChecked = false;
            RBMonthly.IsChecked = false;

            switch (repeatInterval)
            {
                case RepeatInterval.None:
                    RBNone.IsChecked = true;
                    break;
                case RepeatInterval.Weekly:
                    RBWeekly.IsChecked = true;
                    break;
                case RepeatInterval.BiWeekly:
                    RBBiWeekly.IsChecked = true;
                    break;
                case RepeatInterval.Monthly:
                    RBMonthly.IsChecked = true;
                    break;
                default:
                    RBNone.IsChecked = true;
                    break;
            }
        }

        private RepeatInterval GetSelectedRepeatInterval()
        {
            if (RBNone.IsChecked)
                return RepeatInterval.None;
            if (RBWeekly.IsChecked)
                return RepeatInterval.Weekly;
            if (RBBiWeekly.IsChecked)
                return RepeatInterval.BiWeekly;
            if (RBMonthly.IsChecked)
                return RepeatInterval.Monthly;

            return RepeatInterval.None;
        }

        private void SetRadioButtonBasedOnDifficulty(string difficulty)
        {
            RBIniciante.IsChecked = false;
            RBIntermediario.IsChecked = false;
            RBAvançado.IsChecked = false;

            switch (difficulty)
            {
                case "Iniciante":
                    RBIniciante.IsChecked = true;
                    break;
                case "Intermediario":
                    RBIntermediario.IsChecked = true;
                    break;
                case "Avançado":
                    RBAvançado.IsChecked = true;
                    break;
                default:
                    RBIniciante.IsChecked = true;
                    break;
            }
        }

        private string GetSelectedDifficulty()
        {
            if (RBIniciante.IsChecked)
                return "Iniciante";
            if (RBIntermediario.IsChecked)
                return "Intermediario";
            if (RBAvançado.IsChecked)
                return "Avançado";

            return "Default";
        }

        private async Task UploadImage(object sender, EventArgs e)
        {
            foreach (var stream in _tempImageStreams)
            {
                stream.Position = 0;
                string imageUrl = await _azureStorageService.UploadFile(stream);
                _eventToEdit.ImageList.Add(imageUrl);
            }
        }

        private async void OnSelectImageClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.PickAsync();

                if (result != null)
                {
                    var stream = await result.OpenReadAsync();

                    var streamCopy = CopyStream(stream);

                    _imageSources.Add(ImageSource.FromStream(() => CopyStream(stream)));
                    _tempImageStreams.Add(streamCopy);

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Task.Delay(50);
                        ScrollToBottom();
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO: {ex.Message}");
            }
        }

        private Stream CopyStream(Stream input)
        {
            var copy = new MemoryStream();
            input.Position = 0;
            input.CopyTo(copy);
            copy.Position = 0;
            return copy;
        }

        private async void OnDeleteImageButtonClicked(object sender, EventArgs e)
        {
            if (_eventToEdit.ImageList != null && _eventToEdit.ImageList.Any())
            {
                bool userConfirmed = await DisplayAlert("Confirmação", "Tem certeza de que deseja excluir esta imagem?", "Sim", "Não");

                if (userConfirmed)
                {
                    await Navigation.PushPopupAsync(new Loading());

                    string imageUrlToDelete = _eventToEdit.ImageList[_currentIndex];

                    await _azureStorageService.DeleteFile(imageUrlToDelete);

                    _eventToEdit.ImageList.RemoveAt(_currentIndex);

                    ImageCarousel.ItemsSource = null;
                    ImageCarousel.ItemsSource = _imageSourceList;

                    if (_imageSourceList.Count < 2)
                    {
                        ArrowButton.IsVisible = false;
                    }

                    if (_imageSourceList.Count < 1)
                    {
                        DeleteButton.IsVisible = false;
                    }

                    _eventToEdit.ImageUrl = JsonConvert.SerializeObject(_eventToEdit.ImageList);

                    ResponseService<Event> responseService = await _eventService.EditEvent(_eventToEdit);

                    if (responseService.IsSuccess)
                    {
                        var navigationStack = Navigation.NavigationStack.ToList();

                        if (navigationStack.Count >= 3)
                        {
                            Navigation.RemovePage(navigationStack[2]);
                        }

                        await Navigation.PushAsync(new EditEvent(_eventToEdit));
                        await Navigation.PopAllPopupAsync();
                        await DisplayAlert("Exclusão de imagem", "Imagem excluida com sucesso!", "OK");
                    }
                    else
                    {
                        // Tratar erros conforme necessário
                    }
                }
            }
        }

        private void OnPreviousButtonClicked(object sender, EventArgs e)
        {
            _currentIndex = (_currentIndex - 1 + _eventToEdit.ImageList.Count) % _eventToEdit.ImageList.Count;

            ImageCarousel.Position = _currentIndex;
        }

        private void OnNextButtonClicked(object sender, EventArgs e)
        {
            _currentIndex = (_currentIndex + 1) % _eventToEdit.ImageList.Count;

            ImageCarousel.Position = _currentIndex;
        }

        private void HideFields()
        {
            if (_imageSourceList.Count < 1)
            {
                ImageLayout.IsVisible = false;
            }

            if (_imageSourceList.Count < 2)
            {
                ArrowButton.IsVisible = false;
            }
            else { ArrowButton.IsVisible = true; }

            if (selectedLocation.Latitude == 0 && selectedLocation.Longitude == 0)
            {
                MapLayout.IsVisible = false;
            }
            else { MapLayout.IsVisible = true; }

            if (_imageSourceList.Count < 1 && selectedLocation.Latitude == 0 && selectedLocation.Longitude == 0)
            {
                Spacer.IsVisible = false;
            }
            else { Spacer.IsVisible = true; }
        }

        private async void OnSelectLocationClicked(object sender, EventArgs e)
        {
            try
            {
                var mapPage = new MapPage();

                var mapPageCompletionSource = new TaskCompletionSource<Position>();

                mapPage.MapPageReady += (s, args) =>
                {
                    mapPageCompletionSource.SetResult(mapPage.SelectedLocation);
                };

                await Navigation.PushAsync(mapPage);

                selectedLocation = await mapPageCompletionSource.Task;

                if (selectedLocation != null)
                {
                    string lat = selectedLocation.Latitude.ToString();
                    string lng = selectedLocation.Longitude.ToString();

                    //TxtLocalizacao.Text = $"{lat}, {lng}";
                    UpdateMapView(selectedLocation);

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Task.Delay(50);
                        ScrollToBottom();
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO: {ex.Message}");
            }
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
    }
}