namespace vicuna_infra.Messaging
{
    using Confluent.Kafka;
    using System;
    using System.Threading.Tasks;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

    public class ReactiveObserver(Action<string> onNext, Action onCompleted, Action<Exception> onError)
        : IObserver<Message<Null, byte[]>>
    {
        public void OnNext(Message<Null, byte[]> value)
        {
            var messageValue = System.Text.Encoding.UTF8.GetString(value.Value);
            onNext?.Invoke(messageValue);
        }

        public void OnCompleted() => onCompleted?.Invoke();

        public void OnError(Exception error) => onError?.Invoke(error);
    }
}