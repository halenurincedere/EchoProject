using Echo.Data.Entities;

namespace Echo.Data.Entities
{
    // This class represents the many-to-many relationship between users and features.
    public class UserFeatureEntity : BaseEntity
    {
        // ID of the user who received the feature
        public Guid UserId { get; set; }

        // ID of the related feature
        public Guid FeatureId { get; set; }

        // Navigation property for the user
        public UserEntity? User { get; set; }

        // Navigation property for the feature
        public FeatureEntity? Feature { get; set; }
    }
}