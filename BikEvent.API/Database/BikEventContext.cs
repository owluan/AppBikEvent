using BikEvent.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BikEvent.API.Database
{
    public class BikEventContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Job> Jobs { get; set; }

        public BikEventContext(DbContextOptions<BikEventContext> options) : base(options)
        {

        }
    }
}
