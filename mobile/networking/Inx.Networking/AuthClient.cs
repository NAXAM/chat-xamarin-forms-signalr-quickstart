using System;
using Inx.Networking.Core;
using System.Threading.Tasks;
using Inx.DTOs;
namespace Inx.Networking
{
    public class AuthClient
    {
        readonly INetworkingClient client;

        public AuthClient(INetworkingClient client)
        {
            this.client = client;
        }

        public async Task<AccessTokenDto> SignInAsync(SignInDto dto)
        {
            return await client.PostAsync<SignInDto, AccessTokenDto>("account/sign-in", dto); ;
        }

        public async Task<AccessTokenDto> SignUpAsync(SignUpDto dto)
        {
            return await client.PostAsync<SignUpDto, AccessTokenDto>("account/sign-up", dto); ;
        }
    }
}
