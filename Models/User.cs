using System.ComponentModel.DataAnnotations;

namespace BetAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        [Range(0, 9999999999999999.99)]
        public decimal Balance { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.Date)]
        public DateTime UpdatedAt { get; set; }
    }
}
