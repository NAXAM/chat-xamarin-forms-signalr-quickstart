using System;
using Inx.Bootstraps.Core;
namespace Inx.Bootstraps.Models
{
    public class MessageModel : ModelBase
    {
        public int ConversationId { get; set; }

        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime CreatedAt { get; set; }

        public FriendModel Sender { get; set; }
    }
}
