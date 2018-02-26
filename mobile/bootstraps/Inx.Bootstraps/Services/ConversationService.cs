using System;
using Inx.Networking;
using System.Threading.Tasks;
using Inx.Bootstraps.Models;
using System.Linq;
namespace Inx.Bootstraps.Services
{
    public interface IConversationService
    {
        Task<ConversationModel> GetDirectConversationAsync(int recipientId);
        Task<MessageModel[]> GetMessagesAsync(int conversationId);
    }

    public class ConversationService : IConversationService
    {
        readonly ConversationClient client;
        readonly MessageClient msgClient;

        public ConversationService(ConversationClient client, MessageClient msgClient)
        {
            this.msgClient = msgClient;
            this.client = client;
        }

        public async Task<ConversationModel> GetDirectConversationAsync(int recipientId)
        {
            var dto = await client.GetDirectConversationAsync(recipientId);

            if (dto == null)
            {
                return null;
            }

            return new ConversationModel
            {
                Id = dto.Id,
                Title = dto.Title
            };
        }

        public async Task<MessageModel[]> GetMessagesAsync(int conversationId)
        {
            var dtos = await msgClient.GetMessagesByConversationAsync(conversationId);

            return dtos.Select(x => new MessageModel
            {
                Id = x.Id,
                Text = x.Text,
                Sender = new FriendModel
                {
                    Id = x.Sender.Id,
                    Email = x.Sender.Email
                }
            }).ToArray();
        }
    }
}
