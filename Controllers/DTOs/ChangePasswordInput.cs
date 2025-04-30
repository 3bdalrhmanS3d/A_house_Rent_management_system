using System.ComponentModel.DataAnnotations;

namespace Tasken2.Controllers.DTOs
{
    public class ChangePasswordInput
    {
        [Required, DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required, DataType(DataType.Password), MinLength(6)]
        public string NewPassword { get; set; }

        [Required, DataType(DataType.Password), Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
