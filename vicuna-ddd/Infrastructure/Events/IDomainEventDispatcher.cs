namespace vicuna_ddd.Infrastructure.Events
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync<TEvent>(TEvent @event) where TEvent : class;
    }
}