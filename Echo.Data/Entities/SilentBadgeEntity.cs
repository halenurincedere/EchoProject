namespace Echo.Data.Entities
{
    /// <summary>
    /// Represents a silent badge definition that can be awarded to users.
    /// These badges reflect emotional support, listening, or sharing experiences.
    /// </summary>
    public class SilentBadgeEntity : BaseEntity
    {
        /// <summary>
        /// The short badge message that may appear in the interface. 
        /// Example: "You were there when it mattered."
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The reason or context behind the badge.
        /// Example: "For being a silent listener."
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// The list of user-badge connections (many-to-many).
        /// </summary>
        public ICollection<UserSilentBadgeEntity> UserSilentBadges { get; set; }
    }
}