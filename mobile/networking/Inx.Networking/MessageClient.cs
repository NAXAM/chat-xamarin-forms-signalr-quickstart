using System;
using Inx.Networking.Core;
using System.Threading.Tasks;
using Inx.DTOs;
namespace Inx.Networking
{
    public class MessageClient
    {
        readonly INetworkingClient client;

        public MessageClient(INetworkingClient client)
        {
            this.client = client;
        }

        public async Task<MessageDto[]> GetMessagesByConversationAsync(int conversationId)
        {
            return await client.GetAsync<MessageDto[]>($"conversations/{conversationId}/messages");
        }
    }
}
