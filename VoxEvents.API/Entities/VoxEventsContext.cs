using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxEvents.API.Entities
{
    public class VoxEventsContext : DbContext
    {
        public VoxEventsContext(DbContextOptions<VoxEventsContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        public DbSet<VoxEvent> VoxEvents { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<Venue> Venues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Availability>()
                .HasKey(a => new { a.MemberId, a.VoxEventId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
