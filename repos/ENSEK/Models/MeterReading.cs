using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace ENSEK.Models
{
    [Keyless]
    [Table("MeterReadings")]
    public class MeterReading
    {
        public int AccountId { get; set; }
        public DateTime MeterReadingDateTime { get; set; }
        public string MeterReadValue { get; set; } = string.Empty;
    }
    public sealed class MeterReadingCsvMap : CsvHelper.Configuration.ClassMap<MeterReading>
    {
        public MeterReadingCsvMap()
        {
            string format = "dd/MM/yyyy HH:mm";
            var culture = CultureInfo.InvariantCulture;

            Map(m => m.MeterReadingDateTime).TypeConverterOption.Format(format)
              .TypeConverterOption.CultureInfo(culture).Index(1);
            Map(m => m.MeterReadValue).Index(2);
            Map(m => m.AccountId).Index(0);
        }
    }
}
