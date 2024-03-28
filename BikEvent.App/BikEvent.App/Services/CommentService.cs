using BikEvent.App.Models;
using BikEvent.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BikEvent.App.Services
{
    public class CommentService : Service
    {
        public async Task<ResponseService<List<Comment>>> GetComments(int eventId)
        {
            HttpResponseMessage response = await _client.GetAsync($"{BaseApiUrl}/api/comments?eventId={eventId}");

            ResponseService<List<Comment>> responseService = new ResponseService<List<Comment>>();

            responseService.IsSuccess = response.IsSuccessStatusCode;
            responseService.StatusCode = (int)response.StatusCode;

            if (response.IsSuccessStatusCode)
            {
                responseService.Data = await response.Content.ReadAsAsync<List<Comment>>();

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
                var erros = JsonConvert.DeserializeObject<ResponseService<List<Comment>>>(problemResponse);
                responseService.Errors = erros.Errors;
            }
            return responseService;
        }

        public async Task<ResponseService<Comment>> GetComment(int id)
        {
            HttpResponseMessage response = await _client.GetAsync($"{BaseApiUrl}/api/comments/{id}");

            ResponseService<Comment> responseService = new ResponseService<Comment>();

            responseService.IsSuccess = response.IsSuccessStatusCode;
            responseService.StatusCode = (int)response.StatusCode;

            if (response.IsSuccessStatusCode)
            {
                responseService.Data = await response.Content.ReadAsAsync<Comment>();
            }
            else
            {
                string problemResponse = await response.Content.ReadAsStringAsync();
                var erros = JsonConvert.DeserializeObject<ResponseService<Comment>>(problemResponse);
                responseService.Errors = erros.Errors;
            }
            return responseService;
        }

        public async Task<ResponseService<Comment>> AddComment(Comment comment)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync($"{BaseApiUrl}/api/comments", comment);

            ResponseService<Comment> responseService = new ResponseService<Comment>();

            responseService.IsSuccess = response.IsSuccessStatusCode;
            responseService.StatusCode = (int)response.StatusCode;

            if (response.IsSuccessStatusCode)
            {
                responseService.Data = await response.Content.ReadAsAsync<Comment>();
            }
            else
            {
                string problemResponse = await response.Content.ReadAsStringAsync();
                var erros = JsonConvert.DeserializeObject<ResponseService<Comment>>(problemResponse);
                responseService.Errors = erros.Errors;
            }
            return responseService;
        }

        public async Task<ResponseService<Comment>> EditComment(Comment comment)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync($"{BaseApiUrl}/api/comments/", comment);

            ResponseService<Comment> responseService = new ResponseService<Comment>();

            responseService.IsSuccess = response.IsSuccessStatusCode;
            responseService.StatusCode = (int)response.StatusCode;

            if (response.IsSuccessStatusCode)
            {
                responseService.Data = await response.Content.ReadAsAsync<Comment>();
            }
            else
            {
                string problemResponse = await response.Content.ReadAsStringAsync();
                var erros = JsonConvert.DeserializeObject<ResponseService<Comment>>(problemResponse);
                responseService.Errors = erros.Errors;
            }
            return responseService;
        }

        public async Task<ResponseService<Comment>> DeleteComment(int id)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"{BaseApiUrl}/api/comments/{id}");

            ResponseService<Comment> responseService = new ResponseService<Comment>();

            responseService.IsSuccess = response.IsSuccessStatusCode;
            responseService.StatusCode = (int)response.StatusCode;

            if (response.IsSuccessStatusCode)
            {
                responseService.Data = await response.Content.ReadAsAsync<Comment>();
            }
            else
            {
                string problemResponse = await response.Content.ReadAsStringAsync();
                var erros = JsonConvert.DeserializeObject<ResponseService<Comment>>(problemResponse);
                responseService.Errors = erros.Errors;
            }
            return responseService;
        }
    }
}
