namespace vicuna_infra.Messaging
{
    public class Unsubscriber(Action unsubscribeAction) : IDisposable
    {
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                unsubscribeAction?.Invoke();
            }
        }
    }
}