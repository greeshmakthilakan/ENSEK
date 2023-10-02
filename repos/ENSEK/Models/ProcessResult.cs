using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENSEK.Models
{
    [Keyless]
    public class ProcessResult 
    {
        private bool _isSuccess;
        public bool Result { get { return _isSuccess; } set { _isSuccess = value; IsSuccessString = value == true ? "Success" : "Failed"; } }
        [NotMapped]
        public string? IsSuccessString { get; set; }
        public string? Message { get; set; }
        [NotMapped]
        public int Account { get; set; }
        [NotMapped]
        public DateTime ReadDate { get; set; }
        [NotMapped]
        public string? ReadValue { get; set; }
    }

    public class ProcessCount
    {
        public int SuccessCount { get; set; }
        public int FailCount { get; set; }
        public List<ProcessResult>? Results { get; set; }
    }
}
