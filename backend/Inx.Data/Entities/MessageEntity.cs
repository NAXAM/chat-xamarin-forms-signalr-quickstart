using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inx.Data.Entities
{
    [Table("Messages")]
    public class MessageEntity
    {
        [Key]
        public int Id { get; set; }

        public string Text { get; set; }

        public int SenderId { get; set; }

        public int ConversationId { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual UserEntity Sender { get; set; }

        [ForeignKey(nameof(ConversationId))]
        public virtual ConversationEntity Conversation { get; set; }
    }
}
