using System.ComponentModel.DataAnnotations;

namespace Tasken2.Controllers.DTOs
{
    public class ProfileInput
    {
        public int PersonId { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Required, RegularExpression("^[0-9]{11}$", ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
