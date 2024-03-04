using BikEvent.App.Services.Interfaces;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(BikEvent.App.Droid.Services.FilePickerServiceAndroid))]
namespace BikEvent.App.Droid.Services
{
    public class FilePickerServiceAndroid : IFilePickerService
    {
        public async Task<Stream> GetFileStream(string contentUri)
        {
            var context = Android.App.Application.Context;
            using (var resolver = context.ContentResolver)
            {
                var inputStream = resolver.OpenInputStream(Android.Net.Uri.Parse(contentUri));
                var memoryStream = new MemoryStream();
                await inputStream.CopyToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return memoryStream;
            }
        }
    }
}