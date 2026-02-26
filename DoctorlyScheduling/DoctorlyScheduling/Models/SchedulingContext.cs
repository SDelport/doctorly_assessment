using Microsoft.EntityFrameworkCore;

namespace DoctorlyScheduling.Models
{
    public class SchedulingContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Attendee> Attendees { get; set; }

        public SchedulingContext(DbContextOptions<SchedulingContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            base.OnConfiguring(options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .Property(e => e.Version)
                .IsConcurrencyToken();


            base.OnModelCreating(modelBuilder);
        }
    }
}
