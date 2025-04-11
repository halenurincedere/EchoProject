using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Echo.Data.Entities
{
    // Represents a meaningful message or feature shown to users
    public class FeatureEntity : BaseEntity
    {
        [MaxLength(100)] 
        public string Title { get; set; } = string.Empty; // Short, impactful title (e.g. "Just listening is support")

        [MaxLength(500)] 
        public string? Description { get; set; }          // Optional description to give more context

        [MaxLength(300)] 
        public string? Note { get; set; }                 // Reflective note or insight (e.g. "Listening heals")

        [MaxLength(300)] 
        public string? Source { get; set; }               // Who wrote or inspired this message (e.g. Echo Team)

        [MaxLength(100)] 
        public string? Tag { get; set; }                  // Tag for categorization (e.g. empathy, support)

        public bool IsPublic { get; set; } = true;        // Can everyone see this feature?
        public bool IsActive { get; set; } = true;        // Is it currently shown in the app?

        // Many-to-many relation with users who received or interacted with this feature
        public ICollection<UserFeatureEntity> UserFeatures { get; set; } = new HashSet<UserFeatureEntity>();

        // Fluent API configuration (called in DbContext)
        public static void Configure(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<FeatureEntity>();

            entity.ToTable("Features");
            entity.HasKey(f => f.Id);

            entity.Property(f => f.Title)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(f => f.Description)
                  .HasMaxLength(500);

            entity.Property(f => f.Note)
                  .HasMaxLength(300);

            entity.Property(f => f.Source)
                  .HasMaxLength(300);

            entity.Property(f => f.Tag)
                  .HasMaxLength(100);

            entity.Property(f => f.IsPublic)
                  .HasDefaultValue(true);

            entity.Property(f => f.IsActive)
                  .HasDefaultValue(true);
        }
    }
}