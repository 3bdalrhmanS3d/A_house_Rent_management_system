using System.ComponentModel.DataAnnotations;
using Tasken2.Models;

namespace Tasken2.Models
{
    public class Area
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string AreaName { get; set; }
        [Required]
        public string SurroundingArea { get; set; }
    }
}
