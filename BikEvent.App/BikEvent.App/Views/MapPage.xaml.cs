using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace BikEvent.App.Views
{
    public partial class MapPage : ContentPage
    {
        public event EventHandler<MapPageEventArgs> MapPageReady;

        private Map map;

        public Position SelectedLocation { get; set; }

        public MapPage()
        {
            InitializeComponent();

            map = new Map
            {
                MapType = MapType.Street,
                IsShowingUser = true
            };

            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(map);

            map.MapClicked += OnMapClicked;

            Content = stack;
        }

        private void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            SelectedLocation = e.Position;
            MapPageReady?.Invoke(this, new MapPageEventArgs());
            Navigation.PopAsync();
        }
    }

    public class MapPageEventArgs : EventArgs
    {
        // Adicionar informações adicionais, se necessário
    }
}
