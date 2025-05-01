using System.ComponentModel.DataAnnotations;

namespace Tasken2.Controllers.DTOs
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "Please enter your National ID.")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "National ID must be exactly 14 digits.")]
        public string NationalID { get; set; }
    }
}
