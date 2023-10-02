using ENSEK.Models;

namespace ENSEK.Services.CSVServices
{
    public interface ICSVService
    {
        public IEnumerable<T> GetCSV<T>(string csvFilePath);
        public IEnumerable<T> ReadCSV<T>(Stream csvFile);
        public void AddToAccount(IEnumerable<Account> accounts);
        public ProcessCount AddToMeterReading(IEnumerable<MeterReading> readings);
        
    }
}
