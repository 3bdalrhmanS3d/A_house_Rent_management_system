using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasken2.Models
{
    public class Comments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }

        [Required]
        [StringLength(500)]
        public string CommentText { get; set; }

        [Required]
        public DateTime CommentTime { get; set; }

        [Required]
        public int PropertyId { get; set; }
        public Property Property { get; set; }

        [Required]
        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
