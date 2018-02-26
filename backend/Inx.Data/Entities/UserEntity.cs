using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inx.Data.Entities
{
    public class UserEntity : IdentityUser<int>
    {
        [ForeignKey(nameof(MessageEntity.SenderId))]
        public virtual IEnumerable<MessageEntity> Messages { get; set; }

        [ForeignKey(nameof(ConversationEntity.CreatedById))]
        public virtual IEnumerable<ConversationEntity> Conversations { get; set; }

        [ForeignKey(nameof(ConversationUserEntity.UserId))]
        public virtual IEnumerable<ConversationUserEntity> ConversationUsers { get; set; }

        [ForeignKey(nameof(UserDeviceEntity.UserId))]
        public virtual IEnumerable<UserDeviceEntity> Devices { get; set; }

        [ForeignKey(nameof(UserConnectionEntity.UserId))]
        public virtual IEnumerable<UserConnectionEntity> Connections { get; set; }

        [ForeignKey(nameof(ProfileEntity.UserId))]
        public virtual IEnumerable<ProfileEntity> Profile { get; set; }
    }
}
