using System;
using Inx.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Inx.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Inx.Data.Entities;
using System.Runtime.InteropServices;
using Inx.WebAPI.Controllers.Core;

namespace Inx.WebAPI.Controllers
{
    [Authorize]
    [Route("api/users")]
    public class UserController : InxControllerBase
    {
        readonly InxDbContext context;

        public UserController(InxDbContext context, UserManager<UserEntity> userManager) : base(userManager)
        {
            this.context = context;
        }

        [HttpGet("my-friends")]
        public async Task<UserDto[]> GetMyFriendsAsync()
        {
            var currentUser = await GetCurrentUserAsync();
            var currentUserId = currentUser.Id;

            return await Task.Run(delegate
            {
                var users = context.Users.Where(x => x.Id != currentUserId)
                               .Select(x => x.ToDto());

                return users.ToArray();
            });
        }

        [HttpGet("me")]
        public async Task<ProfileDto> GetMyProfileAsync()
        {
            var currentUser = await GetCurrentUserAsync();

            return currentUser.ToProfileDto();
        }
    }
}
