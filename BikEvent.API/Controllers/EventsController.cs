using BikEvent.API.Database;
using BikEvent.Domain.Models;
using BikEvent.Domain.Utility.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            var today = DateTime.Today;

            var events = _context.Events
                .Where(x =>
                    x.PublicationDate >= DateTime.Now.AddYears(-15) &&
                    x.CityState.ToLower().Contains(cityState.ToLower()) &&
                    (
                       x.EventTitle.ToLower().Contains(word.ToLower()) ||
                       x.Tag.ToLower().Contains(word.ToLower()) ||
                       x.Company.ToLower().Contains(word.ToLower())
                    )
                )
                .ToList();

            foreach (var @event in events)
            {
                if (@event.RepeatInterval != RepeatInterval.None && @event.NextEventDate < today)
                {
                    @event.NextEventDate = CalculateNextEventDate(@event.RepeatInterval, @event.NextEventDate);
                    _context.SaveChanges();
                }
            }

            var totalItems = events.Count;

            Response.Headers.Add("X-Total-Items", totalItems.ToString());

            return events
                .Skip(recordsPerPage * (pageNumber - 1))
                .Take(recordsPerPage);
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

        [HttpGet("myevents")]
        public IEnumerable<Event> GetEventsByUser(int userId, string word, string cityState, int pageNumber = 1)
        {
            if (word == null)
                word = string.Empty;

            if (cityState == null)
                cityState = string.Empty;

            var today = DateTime.Today;

            var events = _context.Events
                .Where(x => x.UserId == userId &&
                    x.PublicationDate >= DateTime.Now.AddYears(-15) &&
                    x.CityState.ToLower().Contains(cityState.ToLower()) &&
                    (
                       x.EventTitle.ToLower().Contains(word.ToLower()) ||
                       x.Tag.ToLower().Contains(word.ToLower()) ||
                       x.Company.ToLower().Contains(word.ToLower())
                    )
                )
                .ToList();

            foreach (var @event in events)
            {
                if (@event.RepeatInterval != RepeatInterval.None && @event.NextEventDate < today)
                {
                    @event.NextEventDate = CalculateNextEventDate(@event.RepeatInterval, @event.NextEventDate);
                    _context.SaveChanges();
                }
            }

            var totalItems = events.Count;

            Response.Headers.Add("X-Total-Items", totalItems.ToString());

            return events
                .Skip(recordsPerPage * (pageNumber - 1))
                .Take(recordsPerPage);
        }

        [HttpPost]
        public IActionResult AddEvent(Event _event)
        {
            _event.PublicationDate = DateTime.Now;
            _context.AddAsync(_event);
            _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = _event.Id }, _event);
        }

        [HttpPut]
        public IActionResult EditEvent(Event _event)
        {
            _context.Update(_event);
            _context.SaveChangesAsync();
            return Ok(_event);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEvent(int id)
        {
            var _event = _context.Events.FirstOrDefault(e => e.Id == id);
            _context.Remove(_event);
            _context.SaveChangesAsync();

            return Ok();
        }

        private DateTime CalculateNextEventDate(RepeatInterval repeatInterval, DateTime currentEventDate)
        {
            DateTime newDateTime = currentEventDate;

            switch (repeatInterval)
            {
                case RepeatInterval.Weekly:
                    newDateTime = currentEventDate.AddDays(7);
                    break;

                case RepeatInterval.BiWeekly:
                    newDateTime = currentEventDate.AddDays(14);
                    break;

                case RepeatInterval.Monthly:
                    newDateTime = currentEventDate.AddMonths(1);
                    break;

                default:
                    break;
            }

            return newDateTime;
        }
    }
}
