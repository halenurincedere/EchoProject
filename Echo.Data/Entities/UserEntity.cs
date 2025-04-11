using Echo.Data.Enums;

namespace Echo.Data.Entities
{
    public class UserEntity : BaseEntity
    {
        // User's first name
        public string FirstName { get; set; }

        // User's last name
        public string LastName { get; set; }

        // Date of birth
        public DateTime BirthDate { get; set; }

        // Email address (used for login)
        public string Email { get; set; }

        // Hashed version of the password (stored securely)
        public string PasswordHash { get; set; }

        // User role (Admin or regular User)
        public UserRole UserRole { get; set; } = UserRole.User;

        // Features this user has received (one-to-many with FeatureEntity)
        public ICollection<UserFeatureEntity> UserFeatures { get; set; } = new List<UserFeatureEntity>();

        // Messages the user has sent
        public ICollection<MessageEntity> Messages { get; set; }

        // Rooms where the user joined as a speaker
        public ICollection<ConversationRoomEntity> SpeakerRooms { get; set; }

        // Rooms where the user joined as a listener
        public ICollection<ConversationRoomEntity> ListenerRooms { get; set; }

        // Silent badges the user has received
        public ICollection<UserSilentBadgeEntity> UserSilentBadges { get; set; }
    }
}