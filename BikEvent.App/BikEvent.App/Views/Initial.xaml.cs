using BikEvent.App.Models;
using BikEvent.App.Resources.Load;
using BikEvent.App.Services;
using BikEvent.Domain.Models;
using Rg.Plugins.Popup.Extensions;
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
    public partial class Initial : ContentPage
    {
        private EventService _eventService;
        private ObservableCollection<Event> _eventsList;
        private SearchParams _searchParams;
        private int _eventsListFirstRequest;

        public Initial()
        {
            InitializeComponent();
            _eventService = new EventService();
        }

        private void GoVisualizer(object sender, EventArgs e)
        {
            var eventArgs = (TappedEventArgs) e;
            var page = new Visualizer();
            page.BindingContext = eventArgs.Parameter;
            Navigation.PushAsync(page);
        }

        private void GoRegisterEvent(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegisterEvent());
        }

        private void FocusSearch(object sender, EventArgs e)
        {
            TxtWord.Focus();
        }

        private void FocusCityState(object sender, EventArgs e)
        {
            TxtCityState.Focus();
        }

        private void Logout(object sender, EventArgs e)
        {
            App.Current.Properties.Remove("User");
            App.Current.SavePropertiesAsync();
            App.Current.MainPage = new Login();
        }

        private async void Search(object sender, EventArgs e)
        {
            TxtResultsCount.Text = String.Empty;
            Loading.IsVisible = true;
            Loading.IsRunning = true;
            NoResult.IsVisible = false;

            _searchParams = new SearchParams() { Word = TxtWord.Text, CityState = TxtCityState.Text, PageNumber = 1 };

            ResponseService<List<Event>> responseService = await _eventService.GetEvents(_searchParams.Word, _searchParams.CityState, _searchParams.PageNumber);

            if (responseService.IsSuccess)
            {
                _eventsList = new ObservableCollection<Event>(responseService.Data);
                _eventsListFirstRequest = _eventsList.Count();
                EventsList.ItemsSource = _eventsList;
                EventsList.RemainingItemsThreshold = 1;
                TxtResultsCount.Text = $"{responseService.Pagination.TotalItems} resultado(s).";
            }
            else
            {
                await DisplayAlert("Erro", "Oops! Ocorreu um erro inesperado, tente novamente mais tarde.", "OK");
            }

            if (_eventsList.Count == 0)
            {
                NoResult.IsVisible = true;
            }
            else
            {
                NoResult.IsVisible = false;
            }

            Loading.IsVisible = false;
            Loading.IsRunning = false;
        }

        private async void InfinityScroll(object sender, EventArgs e)
        {
            EventsList.RemainingItemsThreshold = -1;
            _searchParams.PageNumber++;

            ResponseService<List<Event>> responseService = await _eventService.GetEvents(_searchParams.Word, _searchParams.CityState, _searchParams.PageNumber);

            if (responseService.IsSuccess)
            {
                var events = responseService.Data;
                foreach (var item in events)
                {
                    _eventsList.Add(item);
                }
                if (_eventsListFirstRequest == events.Count)
                {
                    EventsList.RemainingItemsThreshold = 1;
                }
                else
                {
                    EventsList.RemainingItemsThreshold = -1;
                }
            }
            else
            {
                await DisplayAlert("Erro", "Oops! Ocorreu um erro inesperado, tente novamente mais tarde.", "OK");
                EventsList.RemainingItemsThreshold = 0;
            }            
        }
    }
}