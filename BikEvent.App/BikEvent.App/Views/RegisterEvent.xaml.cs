﻿using BikEvent.App.Models;
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
using Xamarin.Forms.Xaml;

namespace BikEvent.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterEvent : ContentPage
    {
        private EventService _eventService;
        private AzureStorageService _azureStorageService;
        private List<string> _imageUrl { get; set; }
        private List<Stream> _tempImageStreams { get; set; }

        private int _currentIndex;
        private List<ImageSource> _imageSources { get; set; }

        public RegisterEvent()
        {
            InitializeComponent();
            ImageCarousel.ItemsSource = null;
            _eventService = new EventService();
            _azureStorageService = new AzureStorageService();
            _imageUrl = new List<string>();
            _tempImageStreams = new List<Stream>();
            _imageSources = new List<ImageSource>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ImageCarousel.ItemsSource = null;
            ImageCarousel.ItemsSource = _imageSources;
            HideFields();
        }

        private void GoBack(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void Save(object sender, EventArgs e)
        {
            TxtMessages.Text = String.Empty;

            User user = JsonConvert.DeserializeObject<User>((string)App.Current.Properties["User"]);

            await UploadImage(sender, e);

            Event _event = new Event()
            {
                Company = user.Name,
                EventTitle = TxtEventTitle.Text,
                CityState = TxtCityState.Text,
                EventDate = DatePicker.Date,
                NextEventDate = DatePicker.Date,
                RepeatInterval = (RBNone.IsChecked) ? RepeatInterval.None :
                    (RBWeekly.IsChecked) ? RepeatInterval.Weekly :
                    (RBBiWeekly.IsChecked) ? RepeatInterval.BiWeekly :
                    (RBMonthly.IsChecked) ? RepeatInterval.Monthly :
                 RepeatInterval.None,
                //FinalSalary = TextToDoubleConverter.ToDouble(TxtFinalSalary.Text),
                EventType = (RBPedal.IsChecked) ? "Pedal" : (RBTrilha.IsChecked) ? "Trilha" :
                    (RBPasseio.IsChecked) ? "Passeio" : (RBCompeticao.IsChecked) ? "Competição" :
                    (RBEncontro.IsChecked) ? "Encontro" : (RBOutro.IsChecked) ? "Outro" : "Default",
                Tag = TxtTags.Text,
                SocialMedia = TxtSocialMedia.Text,
                Description = TxtDescription.Text,
                Difficulty = (RBIniciante.IsChecked) ? "Iniciante" : (RBIntermediario.IsChecked) ? "Intermediário" :
                    (RBAvançado.IsChecked) ? "Avançado" : "Default",
                Benefits = TxtBenefits.Text,
                PhoneNumber = TxtPhoneNumber.Text,
                ImageUrl = JsonConvert.SerializeObject(_imageUrl),
                PublicationDate = DateTime.Now,
                UserId = user.Id
            };

            await Navigation.PushPopupAsync(new Loading());

            ResponseService<Event> responseService = await _eventService.AddEvent(_event);

            if (responseService.IsSuccess)
            {
                await Navigation.PopAllPopupAsync();
                await DisplayAlert("Cadastro de Evento", "Evento cadastrado com sucesso!", "OK");

                var navigationStack = Navigation.NavigationStack.ToList();
                if (navigationStack.Count >= 2)
                {
                    Navigation.RemovePage(navigationStack[1]);
                }

                await Navigation.PushAsync(new MyEvents());
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

                await Navigation.PopAllPopupAsync();
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

        private async Task UploadImage(object sender, EventArgs e)
        {
            foreach (var stream in _tempImageStreams)
            {
                stream.Position = 0;
                string imageUrl = await _azureStorageService.UploadFile(stream);
                _imageUrl.Add(imageUrl);
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

        private void HideFields()
        {
            if (_imageSources.Count < 2)
            {
                ArrowButton.IsVisible = false;
            }
            else { ArrowButton.IsVisible = true; }

            if (_imageSources.Count < 1)
            {
                ImageLayout.IsVisible = false;
            }
            else { ImageLayout.IsVisible = true; }
        }

        private void OnPreviousButtonClicked(object sender, EventArgs e)
        {
            _currentIndex = (_currentIndex - 1 + _imageSources.Count) % _imageSources.Count;

            ImageCarousel.Position = _currentIndex;
        }

        private void OnNextButtonClicked(object sender, EventArgs e)
        {
            _currentIndex = (_currentIndex + 1) % _imageSources.Count;

            ImageCarousel.Position = _currentIndex;
        }
    }
}