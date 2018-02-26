using System;
namespace Inx.DTOs
{
    public class NewMessageDto
    {
        public int SenderId { get; set; }

        public int ConversationId { get; set; }

        public string Text { get; set; }
    }
}
