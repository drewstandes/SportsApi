using System.ComponentModel.DataAnnotations;

namespace SportsApi.Models
{
    public class Person
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public bool IsAuthorised { get; set; }

        [Required]
        public bool IsValid { get; set; }

        [Required]
        public bool IsEnabled { get; set; }

        public ICollection<Sport> Sports { get; set; }
    }
}
