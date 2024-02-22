using System.Threading.Tasks;
using Xamarin.Essentials;

namespace BikEvent.App.Services.Interfaces
{
    public interface ILocationService
    {
        Task<Location> GetLocationAsync();
    }
}
