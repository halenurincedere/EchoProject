namespace Echo.WebAPI.Models
{
    /// Represents the response returned after a successful login.
    public class LoginResponse
    {
        /// JWT token for the user
        public string Token { get; set; }

        // When the token expires
        public DateTime ExpireDate { get; set; }

        // Login result message
        public string Message { get; set; }
    }
}