using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tasken2.Controllers.DTOs
{
    public class PropertyInput
    {
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
        public List<IFormFile> clientFiles { get; set; } = new List<IFormFile>();

        [NotMapped]
        public IFormFile propImage1File { get; set; }

        [NotMapped]
        public IFormFile propImage2File { get; set; }

        [NotMapped]
        public IFormFile propImage3File { get; set; }

        [NotMapped]
        public IFormFile propImage4File { get; set; }

        [NotMapped]
        public IFormFile propImage5File { get; set; }
        public string propImage1 { get; set; }
        public string propImage2 { get; set; }
        public string propImage3 { get; set; }
        public string? propImage4 { get; set; }
        public string? propImage5 { get; set; }
        public int CreatedIDBy { get; set; }
        public int AreaId { get; set; }
        public int HireStatus { get; set; } = 0;
    }
}
