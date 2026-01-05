namespace vicuna_infra.Messaging
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public DomainEventDispatcher(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

        public async Task DispatchAsync<TEvent>(TEvent @event) where TEvent : class
        {
            using var scope = _scopeFactory.CreateScope();
            var handlers = scope.ServiceProvider.GetServices<IDomainEventHandler<TEvent>>();
            foreach (var handler in handlers)
            {
                await handler.HandleAsync(@event);
            }
        }
    }
}