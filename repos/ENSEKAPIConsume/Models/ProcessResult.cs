
using System.ComponentModel;

namespace ENSEKAPIConsume.Models
{
    public class ProcessResult
    {
        public bool result { get; set; }
        [DisplayName("Process Status")]
        public string? isSuccessString { get; set; }
        [DisplayName("Message")]
        public string? message { get; set; }
        [DisplayName("Account")]
        public int account { get; set; }
        [DisplayName("Read Date and Time")]
        public DateTime readDate { get; set; }
        [DisplayName("Read Value")]
        public string? readValue { get; set; }
    }

    public class Counts
    {
        public int successCount { get; set; }
        public int failCount { get; set; }
        public List<ProcessResult> results { get; set; }
    }
}
