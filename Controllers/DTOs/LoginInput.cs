using System.ComponentModel.DataAnnotations;

namespace Tasken2.Controllers.DTOs
{
    public class LoginInput
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
