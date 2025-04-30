using System.ComponentModel.DataAnnotations;
using Tasken2.Models;

namespace Tasken2.Controllers.DTOs
{
    public class PropertyEditInput
    {
        public int PropertyId { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public float Area { get; set; }

        [Required]
        public int NumberOfRooms { get; set; }

        [Required, StringLength(50)]
        public string Region { get; set; }

        [Required, StringLength(50)]
        public string Street { get; set; }

        [Required]
        public int FloorNumber { get; set; }

        [Required]
        public int AreaId { get; set; }

        public IFormFile[] ImageFiles { get; set; }
    }

    public class AccountViewModel
    {
        public Person Person { get; set; }
        public List<Property> ActivePosts { get; set; }
        public List<Property> InactivePosts { get; set; }
    }
}
