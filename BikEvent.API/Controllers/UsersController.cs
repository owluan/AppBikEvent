using BikEvent.API.Database;
using BikEvent.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BikEvent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private BikEventContext _context;

        public UsersController(BikEventContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetUser(string email, string password)
        {
            User userDB = _context.Users.FirstOrDefault(x => x.Email == email && x.Password == password);
            
            if (userDB == null)
            {
                return NotFound();
            }

            return new JsonResult(userDB);
        }

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            _context.Users.AddAsync(user);
            _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { email = user.Email, password = user.Password}, user);
        }
    }
}
