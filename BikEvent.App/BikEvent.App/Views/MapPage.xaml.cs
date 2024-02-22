using BikEvent.App.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace BikEvent.App.Views
{
    public partial class MapPage : ContentPage
    {
        public event EventHandler<MapPageEventArgs> MapPageReady;

        private ILocationService _locationService;

        private Xamarin.Forms.Maps.Map map;

        public Position SelectedLocation { get; set; }

        public MapPage()
        {
            InitializeComponent();

            _locationService = DependencyService.Get<ILocationService>();

            map = new Xamarin.Forms.Maps.Map
            {
                MapType = MapType.Street,
                IsShowingUser = true
            };

            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(map);

            map.MapClicked += OnMapClicked;

            Content = stack;

            RequestLocationPermission();
        }

        private void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            SelectedLocation = e.Position;
            MapPageReady?.Invoke(this, new MapPageEventArgs());
            Navigation.PopAsync();
        }

        private async void RequestLocationPermission()
        {
            var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            if (status == PermissionStatus.Granted)
            {
                await InitializeMapWithUserLocation();
            }
            else
            {
                await DisplayAlert("Permissão de Localização", "A permissão de localização não foi concedida. Não é possível exibir sua localização atual.", "OK");
            }
        }

        private async Task InitializeMapWithUserLocation()
        {
            var location = await _locationService.GetLocationAsync();

            if (location != null)
            {
                map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromKilometers(1)));
            }
            else
            {
                await DisplayAlert("Erro", "Não foi possível obter a localização atual", "OK");
            }
        }
    }

    public class MapPageEventArgs : EventArgs
    {
        
    }
}
