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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BikEvent.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditEvent : ContentPage
    {
        private EventService _eventService;
        private Event _eventToEdit;
        public event EventHandler EventUpdated;
        private AzureStorageService _azureStorageService;
        private int _currentIndex;

        public EditEvent(Event eventToEdit)
        {
            InitializeComponent();
            _eventService = new EventService();
            _azureStorageService = new AzureStorageService();
            _eventToEdit = eventToEdit;
            ImageCarousel.ItemsSource = null;
            ImageCarousel.ItemsSource = _eventToEdit.ImageList;
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
            // Atualizar RadioButtons
            _eventToEdit.EventType = GetSelectedEventType();
            _eventToEdit.RepeatInterval = GetSelectedRepeatInterval();
            _eventToEdit.Difficulty = GetSelectedDifficulty();

            await Navigation.PushPopupAsync(new Loading());

            // Chamar o serviço para salvar as alterações no evento
            ResponseService<Event> responseService = await _eventService.EditEvent(_eventToEdit);

            if (responseService.IsSuccess)
            {
                await Navigation.PopAllPopupAsync();
                await DisplayAlert("Edição de Evento", "Evento editado com sucesso!", "OK");

                var navigationStack = Navigation.NavigationStack.ToList();

                if (navigationStack.Count >= 2)
                {
                    Navigation.RemovePage(navigationStack[1]);
                    Navigation.RemovePage(navigationStack[2]);
                }

                await Navigation.PushAsync(new EditVisualizer(_eventToEdit));
            }
            else
            {
                // Tratar erros conforme necessário
                // ...
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

        private void SetRadioButtonBasedOnEventType(string eventType)
        {
            // Limpar seleções existentes
            RBPedal.IsChecked = false;
            RBTrilha.IsChecked = false;
            RBPasseio.IsChecked = false;
            RBCompeticao.IsChecked = false;
            RBEncontro.IsChecked = false;
            RBOutro.IsChecked = false;

            // Selecionar o RadioButton com base no EventType
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
                    // Lógica padrão, se necessário
                    break;
            }
        }

        private string GetSelectedEventType()
        {
            // Retornar o EventType com base no RadioButton selecionado
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

            // Lógica padrão, se necessário
            return "Default";
        }

        private void SetRadioButtonBasedOnRepeatInterval(RepeatInterval repeatInterval)
        {
            // Limpar seleções existentes
            RBNone.IsChecked = false;
            RBWeekly.IsChecked = false;
            RBBiWeekly.IsChecked = false;
            RBMonthly.IsChecked = false;

            // Selecionar o RadioButton com base no EventType
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
            // Retornar o RepeatInterval com base no RadioButton selecionado
            if (RBNone.IsChecked)
                return RepeatInterval.None;
            if (RBWeekly.IsChecked)
                return RepeatInterval.Weekly;
            if (RBBiWeekly.IsChecked)
                return RepeatInterval.BiWeekly;
            if (RBMonthly.IsChecked)
                return RepeatInterval.Monthly;

            // Lógica padrão, se necessário
            return RepeatInterval.None;
        }

        private void SetRadioButtonBasedOnDifficulty(string difficulty)
        {
            // Limpar seleções existentes
            RBIniciante.IsChecked = false;
            RBIntermediario.IsChecked = false;
            RBAvançado.IsChecked = false;

            // Selecionar o RadioButton com base no EventType
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
            // Retornar o EventType com base no RadioButton selecionado
            if (RBIniciante.IsChecked)
                return "Iniciante";
            if (RBIntermediario.IsChecked)
                return "Intermediario";
            if (RBAvançado.IsChecked)
                return "Avançado";

            // Lógica padrão, se necessário
            return "Default";
        }

        private async void OnDeleteImageButtonClicked(object sender, EventArgs e)
        {
            if (_eventToEdit.ImageList != null && _eventToEdit.ImageList.Any())
            {
                bool userConfirmed = await DisplayAlert("Confirmação", "Tem certeza de que deseja excluir esta imagem?", "Sim", "Não");

                if (userConfirmed)
                {
                    // Remove a imagem atual da lista
                    string imageUrlToDelete = _eventToEdit.ImageList[_currentIndex];

                    // Exclui a imagem do Azure Storage
                    await _azureStorageService.DeleteFile(imageUrlToDelete);

                    // Remove a imagem da lista
                    _eventToEdit.ImageList.RemoveAt(_currentIndex);

                    // Atualiza o CarouselView com a nova lista de imagens
                    ImageCarousel.ItemsSource = null;
                    ImageCarousel.ItemsSource = _eventToEdit.ImageList;

                    // Caso não haja mais imagens, esconda os botões de navegação
                    if (_eventToEdit.ImageList.Count < 2)
                    {
                        ArrowButton.IsVisible = false;
                    }

                    if (_eventToEdit.ImageList.Count < 1)
                    {
                        DeleteButton.IsVisible = false;
                    }

                    _eventToEdit.ImageUrl = JsonConvert.SerializeObject(_eventToEdit.ImageList);

                    await Navigation.PushPopupAsync(new Loading());

                    ResponseService<Event> responseService = await _eventService.EditEvent(_eventToEdit);

                    if (responseService.IsSuccess)
                    {
                        await Navigation.PopAllPopupAsync();
                        await DisplayAlert("Exclusão de imagem", "Imagem excluida com sucesso!", "OK");

                        var navigationStack = Navigation.NavigationStack.ToList();

                        if (navigationStack.Count >= 2)
                        {
                            Navigation.RemovePage(navigationStack[1]);
                            Navigation.RemovePage(navigationStack[2]);
                        }

                        await Navigation.PushAsync(new EditVisualizer(_eventToEdit));
                    }
                    else
                    {
                        // Tratar erros conforme necessário
                        // ...
                    }
                }
            }
        }

        private void OnPreviousButtonClicked(object sender, EventArgs e)
        {
            // Navegue para a imagem anterior
            _currentIndex = (_currentIndex - 1 + _eventToEdit.ImageList.Count) % _eventToEdit.ImageList.Count;

            // Atualize a posição atual do CarouselView
            ImageCarousel.Position = _currentIndex;
        }

        private void OnNextButtonClicked(object sender, EventArgs e)
        {
            // Navegue para a próxima imagem
            _currentIndex = (_currentIndex + 1) % _eventToEdit.ImageList.Count;

            // Atualize a posição atual do CarouselView
            ImageCarousel.Position = _currentIndex;
        }
    }
}