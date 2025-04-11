using Echo.Data.Enums;

namespace Echo.Business.Jwt
{
    // This DTO contains both user identity information and JWT configuration details
    public class JwtDto
    {
        // Identity (Claims) Information
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public UserRole UserRole { get; set; }

        // JWT Configuration Settings
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpireMinutes { get; set; } = 30; // Default expiration time in minutes
    }
}