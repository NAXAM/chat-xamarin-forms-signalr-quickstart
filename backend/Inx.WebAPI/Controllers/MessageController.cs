using System;
using Inx.WebAPI.Controllers.Core;
using Inx.Repositories;
using Microsoft.AspNetCore.Identity;
using Inx.Data.Entities;
using System.Threading.Tasks;
using Inx.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Inx.WebAPI.Controllers
{
    [Authorize]
    public class MessageController : InxControllerBase
    {
        readonly IMessageRepository messageRepository;

        public MessageController(
            IMessageRepository messageRepository,
            UserManager<UserEntity> userManager)
            : base(userManager)
        {
            this.messageRepository = messageRepository;
        }

        [HttpGet("api/conversations/{conversationId}/messages")]
        public async Task<MessageDto[]> GetConverstationMessagesAsync(int conversationId)
        {
            var currentUser = await GetCurrentUserAsync();

            var messages = messageRepository.GetAsync(currentUser.Id, conversationId);

            return messages.Select(x => x.ToDto()).ToArray();
        }
    }
}
