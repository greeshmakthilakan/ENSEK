using CsvHelper;
using CsvHelper.Configuration;
using ENSEK.DataAccess;
using ENSEK.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Security.Cryptography.Xml;

namespace ENSEK.Services.CSVServices
{
    public class CSVService : ICSVService
    {
        private readonly ENSEKDbContext _ensekDbContext;
        private readonly CsvConfiguration csvconfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null
        };

        public CSVService(ENSEKDbContext ensekDbContext)
        {
            _ensekDbContext = ensekDbContext;
        }
        public IEnumerable<T> GetCSV<T>(string csvFilePath)
        {
            throw new NotImplementedException();
        }

        public  IEnumerable<T> ReadCSV<T>(Stream csvFile)
        {
            try
            {
                
                var reader = new StreamReader(csvFile);
                using (var csv = new CsvReader(reader, csvconfig))
                {
                    csv.Context.RegisterClassMap<MeterReadingCsvMap>();
                    var records = csv.GetRecords<T>().ToList();
                    return records;
                }
                
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

        }
        //public IEnumerable<T> AddToDB<T>(IEnumerable<T> csvRecords)
        //{
            
        //        string typeName = csvRecords.GetType().GetGenericArguments()[0].Name;

        //        switch (typeName.ToLower())
        //        {
        //            case "account":
        //                AddToAccount((IEnumerable<Account>)csvRecords);
        //                break;
        //            case "meterreading":
        //                return (IEnumerable<T>)AddToMeterReading((IEnumerable<MeterReading>) csvRecords);
                        
        //            default:
        //                break;
        //        }
        //    return csvRecords;
        //}


        public void AddToAccount(IEnumerable<Account> accounts)
        {

            _ensekDbContext.Accounts.AddRange(accounts);

            _ensekDbContext.SaveChanges();

            
        }

        public ProcessCount AddToMeterReading(IEnumerable<MeterReading> readings)
        {
            List<ProcessResult> processResults = new List<ProcessResult>();
            foreach(MeterReading reading in readings)
            {
                SqlParameter accountId = new SqlParameter("@AccountId",reading.AccountId);
                SqlParameter readingDate = new SqlParameter("@MeterReadingDateTime", reading.MeterReadingDateTime);
                SqlParameter readingValue = new SqlParameter("@MeterReadingValue", reading.MeterReadValue);
                var result = _ensekDbContext.ProcessResults.FromSqlRaw("EXEC SP_ValidateAndInsert_MeterReading @AccountId,@MeterReadingDateTime,@MeterReadingValue", accountId,readingDate,readingValue).AsEnumerable();
                var result1 = result.FirstOrDefault();
                result1.Account = reading.AccountId;
                result1.ReadDate = reading.MeterReadingDateTime;
                result1.ReadValue = reading.MeterReadValue;
                processResults.Add(result1);

            }
            ProcessCount count = new ProcessCount { SuccessCount = processResults.Where(r => r.Result == true).Count(),FailCount = processResults.Where(r => r.Result == false).Count(),Results = processResults };

            return count;


        }
    }
}
