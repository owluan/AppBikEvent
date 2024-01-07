using BikEvent.API.Database;
using BikEvent.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BikEvent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : Controller
    {
        private int recordsPerPage = 5;
        private BikEventContext _context;

        public JobsController(BikEventContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Job> GetJobs(string word, string cityState, int pageNumber = 1)
        {
            if (word == null)
                word = string.Empty;

            if (cityState == null)
                cityState = string.Empty;

            var totalItems = _context.Jobs
                .Where(x =>
                    x.PublicationDate >= DateTime.Now.AddYears(-15) &&
                    x.CityState.ToLower().Contains(cityState.ToLower()) &&
                    (
                       x.JobTitle.ToLower().Contains(word.ToLower()) ||
                       x.TecnologyTools.ToLower().Contains(word.ToLower()) ||
                       x.Company.ToLower().Contains(word.ToLower())
                    )
                ).Count();

            Response.Headers.Add("X-Total-Items", totalItems.ToString());

            return _context.Jobs
                .Where(x => 
                    x.PublicationDate >= DateTime.Now.AddYears(-15) &&
                    x.CityState.ToLower().Contains(cityState.ToLower()) &&
                    (
                       x.JobTitle.ToLower().Contains(word.ToLower()) ||
                       x.TecnologyTools.ToLower().Contains(word.ToLower()) ||
                       x.Company.ToLower().Contains(word.ToLower())
                    )
                )
                .Skip(recordsPerPage * (pageNumber - 1))
                .Take(recordsPerPage)
                .ToList<Job>();
        }

        [HttpGet("{id}")]
        public IActionResult GetJob(int id)
        {
            Job jobDB = _context.Jobs.Find(id);

            if (jobDB == null)
            {
                return NotFound();
            }

            return new JsonResult(jobDB);
        }

        [HttpPost]
        public IActionResult AddJob(Job job)
        {
            job.PublicationDate = DateTime.Now;
            _context.AddAsync(job);
            _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetJob), new { id = job.Id }, job);
        }
    }
}
