using System;
using System.Collections.Generic;

namespace SignalRApi.Entities.User
{
    public class RoleEntity : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<UserEntity> Users { get; set; }
    }
}
