using System.Reactive.Disposables;
using System.Reactive.Linq;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry.Serdes;
using vicuna_ddd.Domain.Users.Messaging;

namespace vicuna_infra.Messaging
{
    public class ReactiveConsumerBase : IReactiveConsumer, IDisposable
    {
        private readonly IConsumer<Null, string> consumer;
        private readonly CancellationTokenSource cts = new();
        private readonly ILogger<ReactiveConsumerBase>? logger;

        public ReactiveConsumerBase(ExtendedConsumerConfig config, ILogger<ReactiveConsumerBase> logger)
        {
            var builder = new ConsumerBuilder<Null, string>(config);
            builder.SetKeyDeserializer(Deserializers.Null);
            builder.SetValueDeserializer(new JsonDeserializer<string>().AsSyncOverAsync());

            consumer = builder.Build();
            consumer.Subscribe(config.Topic);
        }


        public IObservable<ConsumeResult<Null, string>> ConsumeAsObservable()
        {
            return Observable.Create<ConsumeResult<Null, string>>(async observer =>
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = await Task.Run(() => consumer.Consume(cts.Token));
                        observer.OnNext(consumeResult);
                    }
                    catch (OperationCanceledException o)
                    {
                        // Break the loop
                        logger.LogInformation($"Received message: {o.Message}");
                        break;
                    }
                    catch (ConsumeException e)
                    {
                        logger.LogError($"Consume error occurred: {e.Error.Reason}");
                    }
                    catch (Exception e)
                    {
                        logger.LogError($"Unexpected error occurred: {e.Message}");
                    }
                }

                return Disposable.Create(() => cts.Dispose());
            });
        }

        /// <summary>
        ///     Releases all resources used by the consumer.
        /// </summary>
        public void Dispose()
        {
            consumer.Dispose();
            cts.Dispose();
        }
    }
}