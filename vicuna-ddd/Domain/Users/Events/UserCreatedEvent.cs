using System;

namespace vicuna_ddd.Domain.Users.Events
{
    // Ein einfaches Domain Event für die Erstellung eines Users
    public class UserCreatedEvent
    {
        public Guid UserId { get; }
        public string UserName { get; }
        public DateTime CreatedAt { get; }

        public UserCreatedEvent(Guid userId, string userName, DateTime createdAt)
        {
            UserId = userId;
            UserName = userName;
            CreatedAt = createdAt;
        }
    }
}