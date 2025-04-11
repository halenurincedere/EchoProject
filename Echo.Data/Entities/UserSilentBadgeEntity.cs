using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Echo.Data.Entities
{
    // This entity represents the many-to-many relationship between Users and SilentBadges.
    // Each user can receive multiple silent badges, and each badge can be given to multiple users.
    public class UserSilentBadgeEntity : BaseEntity
    {
        public Guid UserId { get; set; }                         // ID of the user
        public UserEntity User { get; set; }                     // Navigation to user

        public Guid SilentBadgeId { get; set; }                  // ID of the silent badge
        public SilentBadgeEntity SilentBadge { get; set; }       // Navigation to badge

        public string Reason { get; set; } = string.Empty;       // Why this badge was given
    }

    public class UserSilentBadgeConfiguration : BaseConfiguration<UserSilentBadgeEntity>
    {
        public override void Configure(EntityTypeBuilder<UserSilentBadgeEntity> builder)
        {
            // Since we are using composite key, we ignore the inherited Id property
            builder.Ignore(x => x.Id);

            // Composite primary key made up of UserId and SilentBadgeId
            builder.HasKey("UserId", "SilentBadgeId");

            base.Configure(builder);

            // Relationship: User ↔ UserSilentBadge
            builder.HasOne(usb => usb.User)
                   .WithMany(u => u.UserSilentBadges)
                   .HasForeignKey(usb => usb.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Relationship: SilentBadge ↔ UserSilentBadge
            builder.HasOne(usb => usb.SilentBadge)
                   .WithMany(sb => sb.UserSilentBadges)
                   .HasForeignKey(usb => usb.SilentBadgeId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}