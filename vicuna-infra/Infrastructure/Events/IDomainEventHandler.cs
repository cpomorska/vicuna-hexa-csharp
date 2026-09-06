namespace vicuna_ddd.Infrastructure.Events
{
    public interface IDomainEventHandler<TEvent>
    {
        Task HandleAsync(TEvent @event);
    }
}