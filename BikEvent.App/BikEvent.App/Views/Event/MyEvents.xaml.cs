using BikEvent.App.Models;
using BikEvent.App.Resources.Load;
using BikEvent.App.Services;
using BikEvent.Domain.Models;
using Newtonsoft.Json;
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
    public partial class MyEvents : ContentPage
    {
        private EventService _eventService;
        private ObservableCollection<Event> _eventsList;
        private SearchParams _searchParams;
        private int _eventsListFirstRequest;

        public MyEvents()
        {
            InitializeComponent();          
            _eventService = new EventService();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await SearchAsync();
        }

        private void GoVisualizer(object sender, EventArgs e)
        {
            var eventArgs = (TappedEventArgs) e;
            var page = new EditVisualizer((Event)eventArgs.Parameter);
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

        private async void Search(object sender, EventArgs e)
        {
            await SearchAsync();
        }

        private async Task SearchAsync()
        {
            TxtResultsCount.Text = String.Empty;
            Loading.IsVisible = true;
            Loading.IsRunning = true;
            NoResult.IsVisible = false;

            User user = JsonConvert.DeserializeObject<User>((string)App.Current.Properties["User"]);

            _searchParams = new SearchParams() { Word = TxtWord.Text, PageNumber = 1 };

            ResponseService<List<Event>> responseService = await _eventService.GetEventsByUser(user.Id, _searchParams.Word, _searchParams.CityState, _searchParams.PageNumber);

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

        private void OpenMenu(object sender, EventArgs e)
        {
            if (Application.Current.MainPage is MasterDetailPage mainPage)
            {
                mainPage.IsPresented = true;
            }
        }

        private void OnSearchCompleted(object sender, EventArgs e)
        {
            SearchAsync();
        }
    }
}