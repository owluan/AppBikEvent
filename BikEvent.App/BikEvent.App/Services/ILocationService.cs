using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace BikEvent.App.Services
{
    public interface ILocationService
    {
        Task<Location> GetLocationAsync();
    }
}
