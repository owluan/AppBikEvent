using BikEvent.App.Services.Interfaces;
using BikEvent.App.Droid.Services;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocationService))]
namespace BikEvent.App.Droid.Services
{
    public class LocationService : ILocationService
    {
        public async Task<Location> GetLocationAsync()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Best);
            var location = await Geolocation.GetLocationAsync(request);
            return location;

        }
    }
}