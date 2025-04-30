using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tasken2.Models;

namespace Tasken2.Models
{
    public class Property
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PropertyId { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        [Required]
        public float Area { get; set; }

        [Required]
        public int NumberOfRooms { get; set; }

        [Required]
        [StringLength(50)]
        public string Region { get; set; }

        [Required]
        [StringLength(50)]
        public string Street { get; set; }

        [Required]
        public int FloorNumber { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string? Image4 { get; set; }
        public string? Image5 { get; set; }

        [Required]
        public int CreatedById { get; set; }
        public Person CreatedBy { get; set; }

        [Required]
        public int AreaId { get; set; }
        public Area LocationAreaId { get; set; }

        public int HireStatus { get; set; }

        // Navigation properties
        public ICollection<PropertyRating> PropertyRatings { get; set; }
        public ICollection<Comments> Comments { get; set; }

        public Property()
        {
            CreatedAt = DateTime.UtcNow;
            PropertyRatings = new HashSet<PropertyRating>();
            Comments = new HashSet<Comments>();
            HireStatus = 0;
        }
    }
}
