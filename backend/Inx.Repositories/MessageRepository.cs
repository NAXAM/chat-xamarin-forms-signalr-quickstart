using System;
using Inx.Data;
using System.Threading.Tasks;
using Inx.Data.Entities;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Inx.Repositories
{
    public interface IMessageRepository
    {
        MessageEntity CreateAsync(int conversationId, int senderId, string message);
        MessageEntity[] GetAsync(int userId, int conversationId);
    }

    public class MessageRepository : IMessageRepository
    {
        readonly InxDbContext context;

        public MessageRepository(InxDbContext context)
        {
            this.context = context;
        }

        public MessageEntity CreateAsync(int conversationId, int senderId, string message)
        {
            var msg = new MessageEntity
            {
                ConversationId = conversationId,
                CreatedAt = DateTime.Now,
                SenderId = senderId,
                Text = message
            };
            context.Messages.Add(msg);
            context.SaveChanges();

            return context.Messages
                          .Include(x => x.Sender)
                          .FirstOrDefault(x => x.Id == msg.Id);
        }

        public MessageEntity[] GetAsync(int userId, int conversationId)
        {
            return context.Messages
                          .Include(x => x.Sender)
                          .Where(x => x.ConversationId == conversationId && x.Conversation.ConversationUsers.Any(u => u.UserId == userId))
                          .ToArray();
        }
    }
}
