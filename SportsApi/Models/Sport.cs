using System.ComponentModel.DataAnnotations;

namespace SportsApi.Models
{
    public class Sport
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public bool IsEnabled { get; set; }

        public ICollection<Person> People { get; set; }
    }
}
