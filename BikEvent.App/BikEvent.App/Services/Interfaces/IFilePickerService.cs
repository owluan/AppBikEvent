using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BikEvent.App.Services.Interfaces
{
    public interface IFilePickerService
    {
        Task<Stream> GetFileStream(string contentUri);
    }
}
