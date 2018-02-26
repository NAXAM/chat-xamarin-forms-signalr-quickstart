using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inx.Data.Entities
{
    [Table("Profiles")]
    public class ProfileEntity
    {
        [Key]
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string AvatarUrl { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual UserEntity User { get; set; }
    }
}
