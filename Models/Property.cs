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
        public int propertyID { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal propPrice { get; set; }

        [Required]
        public float propArea { get; set; }

        [Required]
        public int probNumberOfRooms { get; set; }

        [Required]
        [StringLength(50)]
        public string propRegion { get; set; }

        [Required]
        [StringLength(50)]
        public string propStreet { get; set; }

        [Required]
        public int propFloorNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [NotMapped]
        public IFormFile clientFile { get; set; }

        public string propImage1 { get; set; }
        public string propImage2 { get; set; }
        public string propImage3 { get; set; }
        public string? propImage4 { get; set; }
        public string? propImage5 { get; set; }

        [ForeignKey("CreatedBy")]
        public int CreatedIDBy { get; set; }
        public Person CreatedBy { get; set; }

        [ForeignKey("Area")]
        public int AreaId { get; set; }
        public Area Area { get; set; }

        public int HireStatus { get; set; } = 0;

        // Navigation properties
        public ICollection<PropertyRating>? PropertyRatings { get; set; }
        public ICollection<Comments>? comments { get; set; }
    }
}
