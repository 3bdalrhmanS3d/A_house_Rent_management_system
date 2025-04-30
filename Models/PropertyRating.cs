using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasken2.Models
{
    public class PropertyRating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PropertyRatingId { get; set; }

        [Required]
        public int PropertyId { get; set; }
        public Property Property { get; set; }

        [Required]
        public int PersonId { get; set; }
        public Person? Person { get; set; }

        [Required]
        [Range(0, 5)]
        public float Rating { get; set; }

        public DateTime CreatedAt { get; set; }

        public PropertyRating()
        {
            CreatedAt = DateTime.UtcNow;
        }
    }
}
