using System;
using Inx.Networking;
using Inx.Bootstraps.Managers;
using System.Threading.Tasks;
using Inx.Bootstraps.Models;

namespace Inx.Bootstraps.Services
{
    public interface IAuthService
    {
        Task<bool> SignInAsync(string email, string password);
        Task<bool> SignUpAsync(string email, string password);
    }

    public class AuthService : IAuthService
    {
        readonly AuthClient authClient;
        readonly IAuthManager authManager;
        readonly ICurrentUserManager currentUserManager;
        readonly UserClient client;

        public AuthService(
            AuthClient authClient, UserClient client,
            IAuthManager authManager, ICurrentUserManager currentUserManager)
        {
            this.client = client;
            this.currentUserManager = currentUserManager;
            this.authManager = authManager;
            this.authClient = authClient;
        }

        public async Task<bool> SignInAsync(string email, string password)
        {
            var result = await authClient.SignInAsync(new DTOs.SignInDto
            {
                Email = email,
                Password = password
            });

            var ok = result != null;

            if (ok)
            {
                authManager.AuthorizeHeader = new System.Collections.Generic.KeyValuePair<string, string>(
                    result.TokenType, result.AccessToken
                );

                var profileDetails = await client.GetProfileAsync();
                currentUserManager.Profile = new ProfileModel
                {
                    Id = profileDetails.Id,
                    Email = profileDetails.Email
                };
            }

            return ok;
        }

        public async Task<bool> SignUpAsync(string email, string password)
        {
            var result = await authClient.SignUpAsync(new DTOs.SignUpDto
            {
                Email = email,
                Password = password
            });

            var ok = result != null;

            if (ok)
            {
                authManager.AuthorizeHeader = new System.Collections.Generic.KeyValuePair<string, string>(
                    result.TokenType, result.AccessToken
                );

                var profileDetails = await client.GetProfileAsync();
                currentUserManager.Profile = new ProfileModel
                {
                    Id = profileDetails.Id,
                    Email = profileDetails.Email
                };
            }

            return ok;
        }
    }
}
