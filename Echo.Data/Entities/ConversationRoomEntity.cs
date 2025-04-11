namespace Echo.Data.Entities
{
    // Represents a chat room between two users
    public class ConversationRoomEntity : BaseEntity
    {
        public Guid SpeakerId { get; set; }
        public UserEntity Speaker { get; set; }

        public Guid ListenerId { get; set; }
        public UserEntity Listener { get; set; }

        public string SpeakerMode { get; set; }     // e.g. "I want to talk"
        public string ListenerMode { get; set; }    // e.g. "I want to listen"

        public DateTime StartedAt { get; set; }     // Session start time
        public DateTime EndedAt { get; set; }       // Session end time

        public ICollection<MessageEntity> Messages { get; set; } = new List<MessageEntity>();
    }

    // Fluent API configuration for ConversationRoomEntity
    public class ConversationRoomConfiguration : BaseConfiguration<ConversationRoomEntity>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ConversationRoomEntity> builder)
        {
            base.Configure(builder);

            builder.Property(r => r.SpeakerMode)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(r => r.ListenerMode)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(r => r.StartedAt)
                   .IsRequired();

            builder.Property(r => r.EndedAt)
                   .IsRequired();

            builder.HasOne(r => r.Speaker)
                   .WithMany(u => u.SpeakerRooms)
                   .HasForeignKey(r => r.SpeakerId)
                   .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);

            builder.HasOne(r => r.Listener)
                   .WithMany(u => u.ListenerRooms)
                   .HasForeignKey(r => r.ListenerId)
                   .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);

            builder.HasMany(r => r.Messages)
                   .WithOne(m => m.Room)
                   .HasForeignKey(m => m.RoomId)
                   .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade);
        }
    }
}