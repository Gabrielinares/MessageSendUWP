using Microsoft.EntityFrameworkCore;
using MessageSendApi.Models;

namespace MessageSendApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sending>()
                .Property(s => s.IdMessage)
                .HasColumnName("IdMessage")
                .HasColumnType("int");

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Sending> Sendigs { get; set; }

    }
}
