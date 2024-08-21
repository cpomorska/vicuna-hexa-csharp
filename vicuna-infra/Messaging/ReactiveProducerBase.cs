namespace vicuna_infra.Messaging
{
    using Confluent.Kafka;
    using System;
    using System.Reactive.Linq;
    using System.Threading.Tasks;

    public class ReactiveProducerBase : IObservable<Message<Null, byte[]>>
    {
        private readonly string bootstrapServers;
        private readonly ProducerConfig config;
        private IProducer<Null, byte[]> producer;

        public ReactiveProducerBase(string bootstrapServers)
        {
            this.bootstrapServers = bootstrapServers;
            config = new ProducerConfig { BootstrapServers = bootstrapServers };
            producer = new ProducerBuilder<Null, byte[]>(config).Build();
        }

        public IDisposable Subscribe(IObserver<Message<Null, byte[]>> observer)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    Message<Null, byte[]> msg = null;
                    if (msg != null)
                    {
                        await SendAsync(msg);
                    }
                }
            });

            return new Unsubscriber(() => { producer?.Dispose(); });
        }

        private async Task SendAsync(Message<Null, byte[]> message)
        {
            var value = message.Value;
            try
            {
                await producer.ProduceAsync("my-topic", new Message<Null, byte[]> { Value = value });
                Console.WriteLine($"Sent message: {System.Text.Encoding.UTF8.GetString(value)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
            }
        }
    }
}