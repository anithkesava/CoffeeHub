using CoffeHub.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeHub.Repo
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserDetails> UserDetails { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
