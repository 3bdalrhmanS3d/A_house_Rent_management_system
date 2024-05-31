using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Tasken2.Models;

namespace Tasken2.Controllers
{
    public class mypost
    {

        public int propertyID { get; set; }

        
        public decimal propPrice { get; set; }
        public float propArea { get; set; }

        public int probNumberOfRooms { get; set; }

        public string propRegion { get; set; }

        
        public string propStreet { get; set; }

        public int propFloorNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

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
