using BikEvent.API.Database;
using BikEvent.Domain.Models;
using BikEvent.Domain.Utility.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace BikEvent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private int recordsPerPage = 5;
        private BikEventContext _context;

        public CommentsController(BikEventContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Comment> GetComments(int eventId)
        {
            var comments = _context.Comments
                .Where(x =>
                    x.EventId == eventId)
                .ToList();

            var totalItems = comments.Count;

            Response.Headers.Add("X-Total-Items", totalItems.ToString());

            return comments;
        }

        [HttpGet("{id}")]
        public IActionResult GetComment(int id)
        {
            Comment commentDB = _context.Comments.Find(id);

            if (commentDB == null)
            {
                return NotFound();
            }

            return new JsonResult(commentDB);
        }

        [HttpPost]
        public IActionResult AddComment(Comment comment)
        {
            _context.AddAsync(comment);
            _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, comment);
        }

        [HttpPut]
        public IActionResult EditEvent(Event comment)
        {
            _context.Update(comment);
            _context.SaveChangesAsync();
            return Ok(comment);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteComment(int id)
        {
            var comment  = _context.Comments.FirstOrDefault(e => e.Id == id);
            _context.Remove(comment);
            _context.SaveChangesAsync();

            return Ok();
        }
    }
}
