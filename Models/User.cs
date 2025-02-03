using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BlogApp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; } = string.Empty;

        public string Role { get; set; } = "contributor";
    }
}
