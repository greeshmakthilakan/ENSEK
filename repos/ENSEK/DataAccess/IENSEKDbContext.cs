using ENSEK.Models;
using Microsoft.EntityFrameworkCore;

namespace ENSEK.DataAccess
{
    public interface IENSEKDbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<MeterReading> MeterReadings { get; set; }
    }
}
