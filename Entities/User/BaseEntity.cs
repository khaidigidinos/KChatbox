using System;
namespace SignalRApi.Entities.User
{
    public class BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public long CreatedAt { get; set; }
        public long UpdatedAt { get; set; }
    }
}
