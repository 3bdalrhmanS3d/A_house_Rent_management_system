using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasken2.Models
{
    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PersonID { get; set; }  

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } 

        [Required]
        [StringLength(14)]
        public string nationalID { get; set; } 

        [Required]
        [StringLength(14)]
        [RegularExpression(@"^[0-9]{11}$", ErrorMessage = "Invalid Phone Number")]
        public string phoneNumber { get; set; } 

        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(20)]
        public string AccountType { get; set; } = "User";

        public string? NationalIdImage { get; set; }

        public DateTime CreatedAt { get; set; }


        // خصائص التنقل
        public ICollection<Property> Properties { get; set; }
        public ICollection<PropertyRating> PropertyRatings { get; set; }
        public ICollection<Comments> Comments { get; set; }

        public Person()
        {
            CreatedAt = DateTime.UtcNow;
            Properties = new HashSet<Property>();
            PropertyRatings = new HashSet<PropertyRating>();
            Comments = new HashSet<Comments>();
        }
    }
}
