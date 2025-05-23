﻿using BikEvent.App.Models;
using BikEvent.App.Resources.Converters;
using BikEvent.App.Resources.Load;
using BikEvent.App.Services;
using BikEvent.Domain.Models;
using BikEvent.Domain.Utility.Enums;
using FFImageLoading.Work;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        //public event EventHandler<MapPageEventArgs> MapPageReady;

        private int _currentIndex;
        private Position selectedLocation { get; set; }

        public EditEvent(Event eventToEdit)
        {
            InitializeComponent();
            _eventService = new EventService();
            _azureStorageService = new AzureStorageService();
            _eventToEdit = eventToEdit;
            ImageCarousel.ItemsSource = null;
            ImageCarousel.ItemsSource = _eventToEdit.ImageList;
            selectedLocation = new Position(_eventToEdit.Latitude, _eventToEdit.Longitude);
            HideFields();
            FillFieldsWithEventData();
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

            await Navigation.PushPopupAsync(new Loading());

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
            RBPUrbano.IsChecked = false;
            RBTrilha.IsChecked = false;
            RBPasseio.IsChecked = false;
            RBCompeticao.IsChecked = false;
            RBEncontro.IsChecked = false;
            RBOutro.IsChecked = false;

            switch (eventType)
            {
                case "Urbano":
                    RBPUrbano.IsChecked = true;
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
            if (RBPUrbano.IsChecked)
                return "Urbano";
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
                    ImageCarousel.ItemsSource = _eventToEdit.ImageList;

                    if (_eventToEdit.ImageList.Count < 2)
                    {
                        ArrowButtonLeft.IsVisible = false;
                        ArrowButtonRight.IsVisible = false;
                    }

                    if (_eventToEdit.ImageList.Count < 1)
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
            if (ImageCarousel.ItemsSource == null)
            {
                ImageLayout.IsVisible = false;
            }

            if (ImageCarousel.ItemsSource != null && _eventToEdit.ImageList.Count < 1)
            {
                ImageLayout.IsVisible = false;
            }

            if (ImageCarousel.ItemsSource != null && _eventToEdit.ImageList.Count < 2)
            {
                ArrowButtonLeft.IsVisible = false;
                ArrowButtonRight.IsVisible = false;
            }
            else
            {
                ArrowButtonLeft.IsVisible = true;
                ArrowButtonRight.IsVisible = true;
            }

            if (selectedLocation.Latitude == 0 && selectedLocation.Longitude == 0)
            {
                MapLayout.IsVisible = false;
            }
            else { MapLayout.IsVisible = true; }
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