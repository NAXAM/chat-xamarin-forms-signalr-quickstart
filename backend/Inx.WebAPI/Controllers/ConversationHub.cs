using System;
using Microsoft.AspNetCore.SignalR;
using Inx.DTOs;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Inx.Repositories;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;
using System.Linq;
using Inx.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Inx.WebAPI.Controllers.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Inx.WebAPI.Controllers
{
    [Authorize(JwtBearerDefaults.AuthenticationScheme)]
    public class ConversationHub : Hub
    {
        readonly IConversationRepository conversationRepository;
        readonly IMessageRepository messageRepository;
        readonly UserManager<UserEntity> userManager;
        readonly IUserConnectionRepository userConnectionRepository;

        public ConversationHub(
            IConversationRepository conversationRepository,
            IMessageRepository messageRepository,
            IUserConnectionRepository userConnectionRepository,
            UserManager<UserEntity> userManager
        )
        {
            this.userConnectionRepository = userConnectionRepository;
            this.userManager = userManager;
            this.messageRepository = messageRepository;
            this.conversationRepository = conversationRepository;
        }

        public async Task Send(NewMessageDto dto)
        {
            var currentUser = await GetCurrentUserAsync();

            var recipients = conversationRepository.GetConversationUsersAsync(currentUser.Id, dto.ConversationId);

            if (recipients.Length == 0)
            {
                return;
            }

            var message = messageRepository.CreateAsync(dto.ConversationId, currentUser.Id, dto.Text);

            Broastcast("message.sent", message.ToDto(), recipients);
        }

        public async Task StartDirectConverstation(NewConversationDto dto)
        {
            var currentUser = await GetCurrentUserAsync();

            var conversation = conversationRepository.CreateAsync(currentUser.Id, dto.RecipientIds[0], dto.Text);
            var message = conversation.Messages.First();

            Broastcast("conversation.new", conversation.ToDto(), dto.RecipientIds);
            Broastcast("message.sent", message.ToDto(), dto.RecipientIds);
        }

        async void Broastcast<T>(string message, T data, params int[] recipientIds)
        {
            var connections = userConnectionRepository.GetAsync(recipientIds);

            await Clients.Client(Context.ConnectionId).InvokeAsync(message, data);

            Parallel.ForEach(connections, async (connectionId) =>
            {
                await Clients.Client(connectionId).InvokeAsync(message, data);
            });
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            var currentUser = await GetCurrentUserAsync();
            var saved = userConnectionRepository.InsertAsync(
                currentUser.Id,
                Context.ConnectionId
            );
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);

            var currentUser = await GetCurrentUserAsync();
            var saved = userConnectionRepository.RemoveAsync(
                currentUser.Id,
                Context.ConnectionId
            );
        }

        protected async Task<UserEntity> GetCurrentUserAsync()
        {
            return await userManager.GetUserAsync(this.Context.User);
        }
    }

    [Authorize]
    [Route("api/conversations")]
    public class ConversationController : InxControllerBase
    {
        readonly IConversationRepository conversationRepository;

        public ConversationController(
            IConversationRepository conversationRepository,
            UserManager<UserEntity> userManager
        ) : base(userManager)
        {
            this.conversationRepository = conversationRepository;
        }

        [HttpGet("me")]
        public async Task<ConversationDto[]> GetMyConverstationsAsync()
        {
            var currentUser = await GetCurrentUserAsync();

            var conversations = conversationRepository.GetByUserAsync(currentUser.Id);

            return conversations.Select(x => x.ToDto()).ToArray();
        }

        [HttpGet("me/direct/{recipientId}")]
        public async Task<ConversationDto> GetMyConverstationAsync(int recipientId)
        {
            var currentUser = await GetCurrentUserAsync();

            var conversation = conversationRepository.GetByUserAsync(currentUser.Id, recipientId);

            return conversation?.ToDto();
        }
    }

    public static class DTOExtensions
    {
        public static ConversationDto ToDto(this ConversationEntity entity)
        {
            return new ConversationDto
            {
                Id = entity.Id,
                Title = entity.Title,
                LastMessage = entity.Messages?.LastOrDefault().Text
            };
        }

        public static MessageDto ToDto(this MessageEntity entity)
        {
            return new MessageDto
            {
                Id = entity.Id,
                SenderId = entity.SenderId,
                ConversationId = entity.ConversationId,
                Text = entity.Text,
                Sender = entity.Sender.ToDto()
            };
        }

        public static UserDto ToDto(this UserEntity entity)
        {
            return new UserDto
            {
                Id = entity.Id,
                Email = entity.Email
            };
        }

        public static ProfileDto ToProfileDto(this UserEntity entity)
        {
            return new ProfileDto
            {
                Id = entity.Id,
                Email = entity.Email
            };
        }
    }
}
