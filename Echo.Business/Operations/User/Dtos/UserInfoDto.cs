using Echo.Data.Enums;

namespace Echo.Business.Operations.User.Dtos
{
    public class UserInfoDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole UserRole { get; set; }
    }
}