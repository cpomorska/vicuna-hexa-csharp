using System.Text.Json;
using vicuna_ddd.Domain.Messages.Entity;
using vicuna_ddd.Domain.Users.Dto;
using vicuna_ddd.Domain.Users.Messaging;
using vicuna_ddd.Model.Users.Entity;

namespace vicuna_infra.Service
{
    public class MessageDeliveryService(
        IReactiveConsumer consumer,
        ILogger<MessageDeliveryService> logger,
        DeliveryConfirmationProcessor deliveryConfirmationProcessor)
        : IHostedService, IDisposable
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new(); // Configure JSON serializer options if necessary

        public Task StartAsync(CancellationToken cancellationToken)
        {
            consumer.ConsumeAsObservable().Subscribe(result =>
            {
                var messageDeliveredDto = JsonSerializer.Deserialize<DeliveryConfirmationDto>(result.Message.Value, _jsonSerializerOptions);
                logger.LogInformation($"Received MessageDeliveredDto: {JsonSerializer.Serialize(messageDeliveredDto)}");
                deliveryConfirmationProcessor.ProcessMessageDeliveredDto(messageDeliveredDto);
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            consumer.Dispose();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            // The consumer is disposed in the StopAsync method
        }
    }
}