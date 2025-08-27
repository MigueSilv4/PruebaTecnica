using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Models;

namespace PruebaTecnica.Data
{
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Card> Cards => Set<Card>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>().HasIndex(x => x.CardNumber).IsUnique();
            modelBuilder.Entity<Card>().Property(c => c.CardNumber).IsRequired();
            modelBuilder.Entity<Card>().Property(c => c.Id).ValueGeneratedOnAdd();
        }

    }
}
