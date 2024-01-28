using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace BikEvent.App.Services
{
    public class Service
    {
        protected HttpClient _client;
        protected string BaseApiUrl = "https://bikeventapi20240127185233.azurewebsites.net";

        public Service()
        {
            _client = new HttpClient();
        }
    }
}
