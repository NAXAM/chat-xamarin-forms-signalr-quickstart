using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Inx.Data.Entities
{
    [Table("ConversationUsers")]
    public class ConversationUserEntity
    {
        public int UserId { get; set; }

        public int ConversationId { get; set; }

        public virtual UserEntity User { get; set; }

        [ForeignKey(nameof(ConversationId))]
        public virtual ConversationEntity Conversation { get; set; }
    }
}
