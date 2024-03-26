using BikEvent.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BikEvent.API.Database
{
    public class BikEventContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public BikEventContext(DbContextOptions<BikEventContext> options) : base(options)
        {

        }
    }
}
