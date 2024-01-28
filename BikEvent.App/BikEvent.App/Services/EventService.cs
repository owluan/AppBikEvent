using BikEvent.App.Models;
using BikEvent.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace BikEvent.App.Services
{
    public class EventService : Service
    {
        public async Task<ResponseService<List<Event>>> GetEvents(string word, string cityState, int pageNumber = 1)
        {
            HttpResponseMessage response = await _client.GetAsync($"{BaseApiUrl}/api/events?word={word}&cityState={cityState}&pageNumber={pageNumber}");

            ResponseService<List<Event>> responseService = new ResponseService<List<Event>>();

            responseService.IsSuccess = response.IsSuccessStatusCode;
            responseService.StatusCode = (int)response.StatusCode;

            if (response.IsSuccessStatusCode)
            {
                responseService.Data = await response.Content.ReadAsAsync<List<Event>>();

                var pagination = new Pagination()
                {
                    IsPagination = true,
                    TotalItems = int.Parse(response.Headers.GetValues("X-Total-Items").FirstOrDefault())
                };
                responseService.Pagination = pagination;
            }
            else
            {
                string problemResponse = await response.Content.ReadAsStringAsync();
                var erros = JsonConvert.DeserializeObject<ResponseService<List<Event>>>(problemResponse);
                responseService.Errors = erros.Errors;
            }
            return responseService;
        }

        public async Task<ResponseService<List<Event>>> GetEventsByUser(int userId, string word, string cityState, int pageNumber = 1)
        {
            HttpResponseMessage response = await _client.GetAsync($"{BaseApiUrl}/api/events/myevents/?userId={userId}&word={word}&cityState={cityState}&pageNumber={pageNumber}");

            ResponseService<List<Event>> responseService = new ResponseService<List<Event>>();

            responseService.IsSuccess = response.IsSuccessStatusCode;
            responseService.StatusCode = (int)response.StatusCode;

            if (response.IsSuccessStatusCode)
            {
                responseService.Data = await response.Content.ReadAsAsync<List<Event>>();

                var pagination = new Pagination()
                {
                    IsPagination = true,
                    TotalItems = int.Parse(response.Headers.GetValues("X-Total-Items").FirstOrDefault())
                };
                responseService.Pagination = pagination;
            }
            else
            {
                string problemResponse = await response.Content.ReadAsStringAsync();
                var erros = JsonConvert.DeserializeObject<ResponseService<List<Event>>>(problemResponse);
                responseService.Errors = erros.Errors;
            }
            return responseService;
        }

        public async Task<ResponseService<Event>> GetEvent(int id)
        {
            HttpResponseMessage response = await _client.GetAsync($"{BaseApiUrl}/api/events/{id}");

            ResponseService<Event> responseService = new ResponseService<Event>();

            responseService.IsSuccess = response.IsSuccessStatusCode;
            responseService.StatusCode = (int)response.StatusCode;

            if (response.IsSuccessStatusCode)
            {
                responseService.Data = await response.Content.ReadAsAsync<Event>();
            }
            else
            {
                string problemResponse = await response.Content.ReadAsStringAsync();
                var erros = JsonConvert.DeserializeObject<ResponseService<Event>>(problemResponse);
                responseService.Errors = erros.Errors;
            }
            return responseService;
        }

        public async Task<ResponseService<Event>> AddEvent(Event _event)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync($"{BaseApiUrl}/api/events", _event);

            ResponseService<Event> responseService = new ResponseService<Event>();

            responseService.IsSuccess = response.IsSuccessStatusCode;
            responseService.StatusCode = (int)response.StatusCode;

            if (response.IsSuccessStatusCode)
            {
                responseService.Data = await response.Content.ReadAsAsync<Event>();
            }
            else
            {
                string problemResponse = await response.Content.ReadAsStringAsync();
                var erros = JsonConvert.DeserializeObject<ResponseService<Event>>(problemResponse);
                responseService.Errors = erros.Errors;
            }
            return responseService;
        }

        public async Task<ResponseService<Event>> EditEvent(Event _event)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync($"{BaseApiUrl}/api/events/", _event);

            ResponseService<Event> responseService = new ResponseService<Event>();

            responseService.IsSuccess = response.IsSuccessStatusCode;
            responseService.StatusCode = (int)response.StatusCode;

            if (response.IsSuccessStatusCode)
            {
                responseService.Data = await response.Content.ReadAsAsync<Event>();
            }
            else
            {
                string problemResponse = await response.Content.ReadAsStringAsync();
                var erros = JsonConvert.DeserializeObject<ResponseService<Event>>(problemResponse);
                responseService.Errors = erros.Errors;
            }
            return responseService;
        }
    }
}
