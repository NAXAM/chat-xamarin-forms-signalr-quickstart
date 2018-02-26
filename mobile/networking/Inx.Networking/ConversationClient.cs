using Inx.Networking.Core;
using System.Threading.Tasks;
using Inx.DTOs;
namespace Inx.Networking
{
    public class ConversationClient
    {
        readonly INetworkingClient client;

        public ConversationClient(INetworkingClient client)
        {
            this.client = client;
        }

        public async Task<ConversationDto[]> GetConversationsAsync()
        {
            var result = await client.GetAsync<ConversationDto[]>("conversations/me");

            return result;
        }

        public async Task<ConversationDto> GetDirectConversationAsync(int recipientId)
        {
            var result = await client.GetAsync<ConversationDto>($"conversations/me/direct/{recipientId}");

            return result;
        }
    }
}
