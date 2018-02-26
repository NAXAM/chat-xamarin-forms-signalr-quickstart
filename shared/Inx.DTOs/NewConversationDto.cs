namespace Inx.DTOs
{
    public class NewConversationDto
    {
        public int CreatedById { get; set; }

        public int[] RecipientIds { get; set; }

        public string Text { get; set; }
    }
}
