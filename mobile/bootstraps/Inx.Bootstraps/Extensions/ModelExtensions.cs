using System;
using Inx.Bootstraps.Models;
using Inx.DTOs;

namespace Inx.Bootstraps.Extensions
{
    public static class ModelExtensions
    {
        public static FriendModel ToModel(this UserDto dto)
        {
            return new FriendModel
            {
                Id = dto.Id,
                Email = dto.Email
            };
        }

        public static MessageModel ToModel(this MessageDto dto)
        {
            return new MessageModel
            {
                Id = dto.Id,
                CreatedAt = dto.CreatedAt,
                Text = dto.Text,
                ConversationId = dto.ConversationId,
                Sender = dto.Sender?.ToModel()
            };
        }

        public static ConversationModel ToModel(this ConversationDto dto)
        {
            return new ConversationModel
            {
                Id = dto.Id,
                Title = dto.Title,
                LastMessage = dto.LastMessage
            };
        }
    }
}
