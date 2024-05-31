using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasken2.Models
{
    public class Comments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int commentID { get; set; } 

        [Required]
        [StringLength(500)]
        public string commentText { get; set; } = string.Empty; 

        public DateTime commentTime { get; set; } = DateTime.Now;

        [Required]
        [ForeignKey("Property")]
        public int propID { get; set; }
        public Property Property { get; set; } 

        [Required]
        [ForeignKey("Person")]
        public int personID { get; set; } 
        public Person Person { get; set; }
    }
}
