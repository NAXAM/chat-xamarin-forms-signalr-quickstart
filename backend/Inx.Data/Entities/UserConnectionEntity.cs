using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Inx.Data.Entities
{
    [Table("UserConnections")]
    public class UserConnectionEntity
    {
        public int UserId { get; set; }

        public string ConnectionId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual UserEntity User { get; set; }
    }
}
