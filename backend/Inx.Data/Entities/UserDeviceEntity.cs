using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inx.Data.Entities
{
    [Table("UserDevices")]
    public class UserDeviceEntity
    {
        public string DeviceToken { get; set; }

        public int UserId { get; set; }

        public virtual UserEntity User { get; set; }
    }
}
