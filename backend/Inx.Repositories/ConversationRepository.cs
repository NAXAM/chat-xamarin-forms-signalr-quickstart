using Inx.Data;
using System.Threading.Tasks;
using Inx.Data.Entities;
using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace Inx.Repositories
{
    public interface IConversationRepository
    {
        ConversationEntity CreateAsync(int senderId, int receiverId, string text);
        IEnumerable<ConversationEntity> GetByUserAsync(int userId);
        int[] GetConversationUsersAsync(int senderId, int conversationId);
        ConversationEntity GetByUserAsync(int userId, int recipientId);
    }

    public class ConversationRepository : IConversationRepository
    {
        readonly InxDbContext context;

        public ConversationRepository(InxDbContext context)
        {
            this.context = context;
        }

        public ConversationEntity CreateAsync(int senderId, int receiverId, string text)
        {
            var sender = context.Users.FirstOrDefault(x => x.Id == senderId);
            if (sender == null)
            {
                throw new InvalidOperationException("Invalid sender id");
            }

            var receiver = context.Users.FirstOrDefault(x => x.Id == receiverId);
            if (sender == null)
            {
                throw new InvalidOperationException("Invalid receiver id");
            }

            var conversation = new ConversationEntity
            {
                CreatedById = senderId,
                Title = receiver.NormalizedUserName,
                CreatedAt = DateTime.Now,
                ConversationUsers = new List<ConversationUserEntity> {
                        new ConversationUserEntity
                        {
                            UserId = senderId
                        },
                        new ConversationUserEntity
                        {
                            UserId = receiverId
                        },
                    },
                Messages = new List<MessageEntity> {
                        new MessageEntity {
                            SenderId = senderId,
                            Text = text
                        }
                    }
            };

            context.Conversations.Add(conversation);
            context.SaveChanges();

            return conversation;
        }

        public IEnumerable<ConversationEntity> GetByUserAsync(int userId)
        {
            var result = context
                .Conversations
                .Include(x => x.Messages)
                .Where(x => x.ConversationUsers.Any(y => y.UserId == userId));

            return (IEnumerable<ConversationEntity>)result.ToList();
        }

        public ConversationEntity GetByUserAsync(int userId, int recipientId)
        {
            var result = context
                .Conversations
                .Include(x => x.Messages)
                .FirstOrDefault(x => x.ConversationType == ConversationType.Direct
                                && x.ConversationUsers.Any(y => y.UserId == userId)
                                && x.ConversationUsers.Any(y => y.UserId == recipientId));

            return result;
        }

        public int[] GetConversationUsersAsync(int senderId, int conversationId)
        {
            return context.ConversationUsers.Where(x => x.ConversationId == conversationId && x.UserId != senderId)
                          .Select(x => x.UserId).ToArray();
        }
    }
}
