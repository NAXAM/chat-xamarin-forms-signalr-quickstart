using System;
using Inx.Networking.Core;
using System.Threading.Tasks;
using Inx.DTOs;
namespace Inx.Networking
{
    public class UserClient
    {
        readonly INetworkingClient client;

        public UserClient(INetworkingClient client)
        {
            this.client = client;
        }

        public async Task<UserDto[]> GetMyFriendsAsync()
        {
            return await client.GetAsync<UserDto[]>("users/my-friends");
        }

        public async Task<ProfileDto> GetProfileAsync()
        {
            return await client.GetAsync<ProfileDto>("users/me");
        }
    }
}
