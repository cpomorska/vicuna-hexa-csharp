using System.Text;
using Confluent.Kafka;

namespace vicuna_infra.Messaging
{
    public class ReactiveObserver(Action<string> onNext, Action onCompleted, Action<Exception> onError)
        : IObserver<Message<Null, byte[]>>
    {
        public void OnNext(Message<Null, byte[]> value)
        {
            var messageValue = Encoding.UTF8.GetString(value.Value);
            onNext?.Invoke(messageValue);
        }

        public void OnCompleted()
        {
            onCompleted?.Invoke();
        }

        public void OnError(Exception error)
        {
            onError?.Invoke(error);
        }
    }
}