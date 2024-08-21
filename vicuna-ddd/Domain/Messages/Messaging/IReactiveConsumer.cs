using Confluent.Kafka;

namespace vicuna_ddd.Domain.Users.Messaging
{
    /// <summary>
    /// Interface for a reactive consumer that consumes messages as an observable stream.
    /// </summary>
    public interface IReactiveConsumer : IDisposable
    {
        /// <summary>
        /// Returns an observable stream of consumed messages.
        /// </summary>
        /// <returns>An observable sequence of ConsumeResult objects representing the consumed messages.</returns>
        IObservable<ConsumeResult<Null, string>> ConsumeAsObservable();
    }
}