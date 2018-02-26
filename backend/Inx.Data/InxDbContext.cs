using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Inx.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Xml.Linq;

namespace Inx.Data
{
    public class InxDbContext : IdentityDbContext<UserEntity, RoleEntity, int>
    {
        public DbSet<MessageEntity> Messages { get; set; }

        public DbSet<ConversationEntity> Conversations { get; set; }

        public DbSet<ConversationUserEntity> ConversationUsers { get; set; }

        public DbSet<UserDeviceEntity> UserDevices { get; set; }

        public DbSet<UserConnectionEntity> SignalRConnections { get; set; }

        public InxDbContext(DbContextOptions<InxDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<ConversationUserEntity>()
                .HasOne(x => x.User)
                .WithMany(x => x.ConversationUsers)
                .HasForeignKey(x => x.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<ConversationUserEntity>()
                .HasKey(x => new { x.UserId, x.ConversationId });

            builder
                .Entity<UserDeviceEntity>()
                .HasKey(x => new { x.UserId, x.DeviceToken });

            builder
                .Entity<UserConnectionEntity>()
                .HasKey(x => new { x.UserId, x.ConnectionId });

            builder
                .Entity<MessageEntity>()
                .HasOne(x => x.Sender)
                .WithMany(x => x.Messages)
                .HasForeignKey(x => x.SenderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
