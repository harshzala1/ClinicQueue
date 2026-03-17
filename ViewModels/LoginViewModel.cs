using System.ComponentModel.DataAnnotations;

namespace ClinicQueue.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(1, ErrorMessage = "Password cannot be empty")]
        public string Password { get; set; } = string.Empty;

        public string? ErrorMessage { get; set; }
    }
}
