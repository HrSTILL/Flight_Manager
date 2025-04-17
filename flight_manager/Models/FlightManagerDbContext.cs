using Microsoft.EntityFrameworkCore;

namespace flight_manager.Models
{
    public class FlightManagerDbContext : DbContext
    {
        public FlightManagerDbContext(DbContextOptions<FlightManagerDbContext> options) : base(options) { }

        public DbSet<StaffMembers> StaffMembers { get; set; } = null!;
        public DbSet<LoginToken> LoginTokens { get; set; } = null!;
        public DbSet<Flights> Flights { get; set; } = null!;
        public DbSet<Reservations> Reservations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservations>(entity =>
            {
                entity.HasKey(e => e.Reservation_id); 

                entity.Property(e => e.First_Name) 
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Middle_Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Last_Name) 
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Role)
                    .IsRequired();

                entity.Property(e => e.EGN)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Phone_Number)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Nationality)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Ticket_Type)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            
        }
    }
}
