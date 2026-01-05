using System.Threading.Tasks;

namespace vicuna_infra.Messaging
{
    public interface IDomainEventHandler<TEvent>
    {
        Task HandleAsync(TEvent @event);
    }
}