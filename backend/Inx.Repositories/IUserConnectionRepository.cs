using System;
using Inx.Data;
using System.Threading.Tasks;
using System.Linq;
namespace Inx.Repositories
{
    public interface IUserConnectionRepository
    {
        bool InsertAsync(int userId, string connectionId);
        bool RemoveAsync(int userId, string connectionId);
        string[] GetAsync(params int[] userIds);
    }

    public class UserConnectionRepository : IUserConnectionRepository
    {
        readonly InxDbContext context;

        public UserConnectionRepository(InxDbContext context)
        {
            this.context = context;
        }

        public bool InsertAsync(int userId, string connectionId)
        {
            if (context.SignalRConnections.Any(x => x.UserId == userId && x.ConnectionId == connectionId))
            {
                return true;
            }

            context.SignalRConnections.Add(new Data.Entities.UserConnectionEntity()
            {
                ConnectionId = connectionId,
                UserId = userId
            });
            var rowsCount = context.SaveChanges();

            return rowsCount > 0;
        }

        public bool RemoveAsync(int userId, string connectionId)
        {
            var toRemove = context.SignalRConnections.Where(x => x.UserId == userId && x.ConnectionId == connectionId)
                                  .ToArray();

            context.SignalRConnections.RemoveRange(toRemove);

            var rowsCount = context.SaveChanges();

            return rowsCount > 0;
        }


        public string[] GetAsync(params int[] userIds)
        {
            return context.SignalRConnections
                          .Where(x => userIds.Contains(x.UserId))
                          .Select(x => x.ConnectionId)
                          .ToArray();

        }
    }
}
