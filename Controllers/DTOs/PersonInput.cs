using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Tasken2.Controllers.DTOs
{
    public class PersonInput
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PersonID { get; set; }


        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

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

        public IFormFile? NationalIdImageFile { get; set; }


    }
}
