using Echo.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Echo.Data.Contexts
{
    public class EchoDbContext : DbContext
    {
        public EchoDbContext(DbContextOptions<EchoDbContext> options) : base(options) { }

        // DbSets for all entities
        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<FeatureEntity> Features => Set<FeatureEntity>();
        public DbSet<ConversationRoomEntity> Rooms => Set<ConversationRoomEntity>();
        public DbSet<MessageEntity> Messages => Set<MessageEntity>();
        public DbSet<SilentBadgeEntity> SilentBadges => Set<SilentBadgeEntity>();
        public DbSet<UserSilentBadgeEntity> UserSilentBadges => Set<UserSilentBadgeEntity>();
        public DbSet<UserFeatureEntity> UserFeatures => Set<UserFeatureEntity>();
        public DbSet<SettingEntity> Settings => Set<SettingEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /* ───────────────────── SEED DEFAULT SETTING ───────────────────── */
            var settingGuid = Guid.Parse("11111111-1111-1111-1111-111111111111");

            modelBuilder.Entity<SettingEntity>(b =>
            {
                b.Property(x => x.Id).ValueGeneratedNever(); // Guid, not auto-generated
                b.HasData(new SettingEntity
                {
                    Id = settingGuid,
                    MaintenanceMode = false,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                });
            });

            /* ───────────────────── RELATIONSHIPS ───────────────────── */

            // ConversationRoom → Speaker
            modelBuilder.Entity<ConversationRoomEntity>()
                .HasOne(r => r.Speaker)
                .WithMany(u => u.SpeakerRooms)
                .HasForeignKey(r => r.SpeakerId)
                .OnDelete(DeleteBehavior.Restrict);

            // ConversationRoom → Listener
            modelBuilder.Entity<ConversationRoomEntity>()
                .HasOne(r => r.Listener)
                .WithMany(u => u.ListenerRooms)
                .HasForeignKey(r => r.ListenerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Message → Room
            modelBuilder.Entity<MessageEntity>()
                .HasOne(m => m.Room)
                .WithMany(r => r.Messages)
                .HasForeignKey(m => m.RoomId);

            // Message → Sender
            modelBuilder.Entity<MessageEntity>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.SenderId);

            // User ↔ SilentBadge (many-to-many)
            modelBuilder.Entity<UserSilentBadgeEntity>()
                .HasOne(ub => ub.User)
                .WithMany(u => u.UserSilentBadges)
                .HasForeignKey(ub => ub.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserSilentBadgeEntity>()
                .HasOne(ub => ub.SilentBadge)
                .WithMany(sb => sb.UserSilentBadges)
                .HasForeignKey(ub => ub.SilentBadgeId)
                .OnDelete(DeleteBehavior.Cascade);

            // User ↔ Feature (many-to-many)
            modelBuilder.Entity<UserFeatureEntity>()
                .HasKey(uf => new { uf.UserId, uf.FeatureId });

            modelBuilder.Entity<UserFeatureEntity>()
                .HasOne(uf => uf.User)
                .WithMany(u => u.UserFeatures)
                .HasForeignKey(uf => uf.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserFeatureEntity>()
                .HasOne(uf => uf.Feature)
                .WithMany(f => f.UserFeatures)
                .HasForeignKey(uf => uf.FeatureId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
    /// Factory used at design time by EF Core tools to apply migrations
    public class EchoDbContextFactory : IDesignTimeDbContextFactory<EchoDbContext>
    {
        public EchoDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../Echo.WebApi")))
                .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<EchoDbContext>();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("PostgreSql"));

            return new EchoDbContext(optionsBuilder.Options);
        }
    }
}