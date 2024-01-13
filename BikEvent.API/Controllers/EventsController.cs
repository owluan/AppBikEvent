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
    public class EventsController : Controller
    {
        private int recordsPerPage = 5;
        private BikEventContext _context;

        public EventsController(BikEventContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Event> GetEvents(string word, string cityState, int pageNumber = 1)
        {
            if (word == null)
                word = string.Empty;

            if (cityState == null)
                cityState = string.Empty;

            var totalItems = _context.Events
                .Where(x =>
                    x.PublicationDate >= DateTime.Now.AddYears(-15) &&
                    x.CityState.ToLower().Contains(cityState.ToLower()) &&
                    (
                       x.EventTitle.ToLower().Contains(word.ToLower()) ||
                       x.TecnologyTools.ToLower().Contains(word.ToLower()) ||
                       x.Company.ToLower().Contains(word.ToLower())
                    )
                ).Count();

            Response.Headers.Add("X-Total-Items", totalItems.ToString());

            return _context.Events
                .Where(x => 
                    x.PublicationDate >= DateTime.Now.AddYears(-15) &&
                    x.CityState.ToLower().Contains(cityState.ToLower()) &&
                    (
                       x.EventTitle.ToLower().Contains(word.ToLower()) ||
                       x.TecnologyTools.ToLower().Contains(word.ToLower()) ||
                       x.Company.ToLower().Contains(word.ToLower())
                    )
                )
                .Skip(recordsPerPage * (pageNumber - 1))
                .Take(recordsPerPage)
                .ToList<Event>();
        }

        [HttpGet("{id}")]
        public IActionResult GetEvent(int id)
        {
            Event eventDB = _context.Events.Find(id);

            if (eventDB == null)
            {
                return NotFound();
            }

            return new JsonResult(eventDB);
        }

        [HttpPost]
        public IActionResult AddEvent(Event _event)
        {
            _event.PublicationDate = DateTime.Now;
            _context.AddAsync(_event);
            _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = _event.Id }, _event);
        }
    }
}
