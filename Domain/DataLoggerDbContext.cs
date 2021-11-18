using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class DataLoggerDbContext : DbContext
    {
        public virtual DbSet<Measurement> Measurements { get; set; }
        public virtual DbSet<Sensor> Sensors { get; set; }
        public virtual DbSet<OperationTimer> Operationtimer { get; set; }
        public virtual DbSet<Device> DeviceProperties { get; set; }

        public DataLoggerDbContext(DbContextOptions<DataLoggerDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Measurement>()
                .HasOne(m => m.Sensor)
                .WithMany(s => s.Measurements)
                .HasForeignKey(s => s.SensorId);

            modelBuilder.Entity<OperationTimer>();

            modelBuilder.Entity<Device>();
        }
    }
}
