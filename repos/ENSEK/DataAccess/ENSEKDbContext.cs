using ENSEK.Models;
using Microsoft.EntityFrameworkCore;

namespace ENSEK.DataAccess
{
    public class ENSEKDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<MeterReading> MeterReadings { get; set; }
        public DbSet<ProcessResult> ProcessResults { get; set; }
        public ENSEKDbContext(DbContextOptions<ENSEKDbContext> options) : base(options)
        {
        }
        public ENSEKDbContext()
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Account>()
                .HasIndex(u => u.AccountId)
                .IsUnique();
        }
    }
}
