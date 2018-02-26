using System;
namespace Inx.DTOs
{
    public class MessageDto : NewMessageDto
    {
        public int Id { get; set; }

        public UserDto Sender { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
