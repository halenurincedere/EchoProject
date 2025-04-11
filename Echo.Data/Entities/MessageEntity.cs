namespace Echo.Data.Entities
{
    // Represents a single message sent in a conversation room
    public class MessageEntity : BaseEntity
    {
        // The room this message belongs to
        public Guid RoomId { get; set; }
        public ConversationRoomEntity Room { get; set; }

        // The user who sent this message
        public Guid SenderId { get; set; }
        public UserEntity Sender { get; set; }

        // The message text
        public string Content { get; set; }

        // The exact time this message was sent
        public DateTime SentAt { get; set; }
    }

    // Fluent API configuration for the MessageEntity
    public class MessageConfiguration : BaseConfiguration<MessageEntity>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<MessageEntity> builder)
        {
            base.Configure(builder);

            // Content is required and limited to 1000 characters
            builder.Property(m => m.Content)
                   .IsRequired()
                   .HasMaxLength(1000);

            // SentAt is required to track message timing
            builder.Property(m => m.SentAt)
                   .IsRequired();

            // A message belongs to one Room
            builder.HasOne(m => m.Room)
                   .WithMany(r => r.Messages)
                   .HasForeignKey(m => m.RoomId)
                   .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade); // if room is deleted, delete messages too

            // A message is sent by one User
            builder.HasOne(m => m.Sender)
                   .WithMany(u => u.Messages)
                   .HasForeignKey(m => m.SenderId)
                   .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict); // don't delete sender if user is deleted
        }
    }
}