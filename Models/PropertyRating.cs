using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasken2.Models
{
    public class PropertyRating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int propertyRatingID { get; set; } 

        [Required]
        [ForeignKey("Property")]
        public int propID { get; set; } 

        public Property Property { get; set; } 

        [Required]
        [ForeignKey("Person")]
        public int personID { get; set; } 

        public Person Person { get; set; } 

        [Required]
        [Range(0, 5)]
        public float Rating { get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.Now; 
    }
}
