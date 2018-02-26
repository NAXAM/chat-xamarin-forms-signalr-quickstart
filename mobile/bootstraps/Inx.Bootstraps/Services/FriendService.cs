using System;
using Inx.Networking;
using System.Threading.Tasks;
using Inx.Bootstraps.Models;
using System.Security.Cryptography.X509Certificates;
using System.Linq;

namespace Inx.Bootstraps.Services
{
    public interface IFriendService
    {
        Task<FriendModel[]> GetMyFriendsAsync();
    }

    public class FriendService : IFriendService
    {
        readonly UserClient client;

        public FriendService(UserClient client)
        {
            this.client = client;
        }

        public async Task<FriendModel[]> GetMyFriendsAsync()
        {
            var dtos = await client.GetMyFriendsAsync();

            return dtos.Select(x => new FriendModel
            {
                Id = x.Id,
                Email = x.Email
            }).ToArray();
        }
    }
}
