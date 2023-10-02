using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENSEK.Models
{
    [Table("Accounts")]
    public class Account
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        public int AccountId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
