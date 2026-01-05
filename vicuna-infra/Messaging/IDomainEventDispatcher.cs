using System.Threading.Tasks;

namespace vicuna_infra.Messaging
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync<TEvent>(TEvent @event) where TEvent : class;
    }
}