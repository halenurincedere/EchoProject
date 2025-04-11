namespace Echo.Business.Operations.User.Dtos
{
    // This DTO is used when a user tries to log in to the system.
    public class LoginUserDto
    {
        // The email address the user registered with.
        public string Email { get; set; } = null!;

        // The password provided by the user during login.
        public string Password { get; set; } = null!;
    }
}