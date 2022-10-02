namespace com.scprojekt.port.util
{
    public interface IOptional<T>
    {
        void ifPresent();
        void ifPresentOrElse();
        bool isPresent();
        bool isEmpty();
        IOptional<T> of(object value);
        IOptional<T> ofNullable(object value);
        IOptional<T> get();
        IOptional<T> orElse();
        IOptional<T> orElseGet();
    }
}
