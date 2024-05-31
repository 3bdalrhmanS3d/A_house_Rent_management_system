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
        public int personID { get; set; }  

        [Required]
        [StringLength(50)]
        public string firstName { get; set; } 

        [Required]
        [StringLength(50)]
        public string lastName { get; set; } 

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
        [DataType(DataType.Password)]
        public string password { get; set; } 

        [Required]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "Passwords does not match.")]
        public string confirmPassword { get; set; } 

        [DefaultValue("user")]
        public string accountType { get; set; } 

        public string? nationalIdImage { get; set; } 

       
        [NotMapped]
        public IFormFile File { get; set; }
        public DateTime createdAt { get; set; } = DateTime.Now; 

        // خصائص التنقل
        public ICollection<Property>? Properties { get; set; }
        public ICollection<PropertyRating>? PropertyRatings { get; set; }
        public ICollection<Comments>? Comments { get; set; }

    }
}
