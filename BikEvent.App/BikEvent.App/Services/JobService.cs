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
    public class JobService : Service
    {
        public async Task<ResponseService<List<Job>>> GetJobs(string word, string cityState, int pageNumber = 1)
        {
            HttpResponseMessage response = await _client.GetAsync($"{BaseApiUrl}/api/jobs?word={word}&cityState={cityState}&pageNumber={pageNumber}");

            ResponseService<List<Job>> responseService = new ResponseService<List<Job>>();

            responseService.IsSuccess = response.IsSuccessStatusCode;
            responseService.StatusCode = (int)response.StatusCode;

            if (response.IsSuccessStatusCode)
            {
                responseService.Data = await response.Content.ReadAsAsync<List<Job>>();

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
                var erros = JsonConvert.DeserializeObject<ResponseService<List<Job>>>(problemResponse);
                responseService.Errors = erros.Errors;
            }
            return responseService;
        }

        public async Task<ResponseService<Job>> GetJob(int id)
        {
            HttpResponseMessage response = await _client.GetAsync($"{BaseApiUrl}/api/jobs/{id}");

            ResponseService<Job> responseService = new ResponseService<Job>();

            responseService.IsSuccess = response.IsSuccessStatusCode;
            responseService.StatusCode = (int)response.StatusCode;

            if (response.IsSuccessStatusCode)
            {
                responseService.Data = await response.Content.ReadAsAsync<Job>();
            }
            else
            {
                string problemResponse = await response.Content.ReadAsStringAsync();
                var erros = JsonConvert.DeserializeObject<ResponseService<Job>>(problemResponse);
                responseService.Errors = erros.Errors;
            }
            return responseService;
        }

        public async Task<ResponseService<Job>> AddJob(Job job)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync($"{BaseApiUrl}/api/jobs", job);

            ResponseService<Job> responseService = new ResponseService<Job>();

            responseService.IsSuccess = response.IsSuccessStatusCode;
            responseService.StatusCode = (int)response.StatusCode;

            if (response.IsSuccessStatusCode)
            {
                responseService.Data = await response.Content.ReadAsAsync<Job>();
            }
            else
            {
                string problemResponse = await response.Content.ReadAsStringAsync();
                var erros = JsonConvert.DeserializeObject<ResponseService<Job>>(problemResponse);
                responseService.Errors = erros.Errors;
            }
            return responseService;
        }
    }
}
