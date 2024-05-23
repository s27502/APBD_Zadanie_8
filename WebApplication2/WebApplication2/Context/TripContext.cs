using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Context;

public class TripContext : DbContext
    {
        public TripContext(DbContextOptions<TripContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<ClientTrip> ClientTrips { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Pesel).IsRequired().HasMaxLength(11);
            });

            modelBuilder.Entity<Trip>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.StartDate).IsRequired();
                entity.Property(e => e.EndDate).IsRequired();
            });

            modelBuilder.Entity<ClientTrip>(entity =>
            {
                entity.HasKey(e => new { e.ClientId, e.TripId });

                entity.HasOne(ct => ct.Client)
                    .WithMany(c => c.ClientTrips)
                    .HasForeignKey(ct => ct.ClientId);

                entity.HasOne(ct => ct.Trip)
                    .WithMany(t => t.ClientTrips)
                    .HasForeignKey(ct => ct.TripId);

                entity.Property(e => e.RegisteredAt).IsRequired();
            });
        }
    }
