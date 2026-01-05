using System;

namespace vicuna_ddd.Domain.Users.Events
{
    // Ein einfaches Domain Event für die Erstellung eines Users
    public class UserCreatedEvent(Guid userId, string userName, DateTime createdAt)
    {
        public Guid UserId { get; } = userId;
        public string UserName { get; } = userName;
        public DateTime CreatedAt { get; }  = createdAt;
    }
}