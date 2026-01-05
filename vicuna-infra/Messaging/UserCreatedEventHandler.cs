using System.Text.Json;
using Confluent.Kafka;
using vicuna_ddd.Domain.Users.Events;

namespace vicuna_infra.Messaging
{
    public class UserCreatedEventHandler : IDomainEventHandler<UserCreatedEvent>
    {
        private readonly ProducerConfig _producerConfig;
        private readonly string _topic;

        public UserCreatedEventHandler(ProducerConfig producerConfig, IConfiguration cfg)
        {
            _producerConfig = producerConfig;
            _topic = cfg["Kafka:TopicOut"] ?? "user-out";
        }

        public async Task HandleAsync(UserCreatedEvent @event)
        {
            using var producer = new ProducerBuilder<Null, string>(_producerConfig).Build();
            var payload = JsonSerializer.Serialize(@event);
            await producer.ProduceAsync(_topic, new Message<Null, string> { Value = payload });
        }
    }
}