namespace Echo.Business.Jwt
{
    // Holds the claim keys used when generating or reading JWT tokens
    public static class JwtClaimNames
    {
        public const string Id         = "id";
        public const string Email      = "email";
        public const string FirstName  = "firstName";
        public const string LastName   = "lastName";
        public const string UserRole   = "userRole"; // ↪️ Custom claim for role-based access
    }
}