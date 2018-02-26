using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Inx.Data.Entities
{
    [Table("Conversations")]
    public class ConversationEntity
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public ConversationType ConversationType { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedById { get; set; }

        [ForeignKey(nameof(CreatedById))]
        public virtual UserEntity CreatedBy { get; set; }

        [ForeignKey(nameof(MessageEntity.ConversationId))]
        public virtual IList<MessageEntity> Messages { get; set; }

        [ForeignKey(nameof(ConversationUserEntity.ConversationId))]
        public virtual IList<ConversationUserEntity> ConversationUsers { get; set; }
    }
}
