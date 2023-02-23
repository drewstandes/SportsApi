using Microsoft.EntityFrameworkCore;
using SportsApi.Models;

namespace SportsApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options) { }

        public DbSet<Person> People { get; set; }

        public DbSet<Sport> Sports { get; set; }

        // public DbSet<FavouriteSport> FavouriteSports { get; set; }
    }
}
