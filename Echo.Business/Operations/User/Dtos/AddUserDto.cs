using System.ComponentModel.DataAnnotations;
using Echo.Data.Enums;

namespace Echo.Business.Dtos
{
    // This DTO is used to add/register a new user into the system
    public class AddUserDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(50, ErrorMessage = "First name can't exceed 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(50, ErrorMessage = "Last name can't exceed 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Birth date is required.")]
        public DateTime BirthDate { get; set; }

        // Optional: defaults to 'User' if not specified
        public UserRole UserRole { get; set; } = UserRole.User;
    }
}