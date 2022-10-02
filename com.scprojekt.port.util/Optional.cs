namespace com.scprojekt.port.util
{
    public class Optional<T> where T : class
    {
        private static readonly Optional<T> EMPTY = new Optional<T>();
        private readonly T? value;

        private Optional()
        {
            value = default;
        }

        private Optional(T optionalValue)
        {
            value = optionalValue;
        }

        public static Optional<T> Empty()
        {
            return EMPTY;
        }

        public static Optional<T> Of(T optionalValue)
        {
            return new Optional<T>(optionalValue);
        }

        public static Optional<T> OfNullable(T optionalValue)
        {
            return optionalValue != null ? new Optional<T>(optionalValue) : new Optional<T>();
        }

        public static Optional<T> OfNullable(Func<T> outputoptionalValue)
        {
            return outputoptionalValue != null ? Of(outputoptionalValue()) : Empty();
        }

        public bool IsPresent()
        {
            return value != null;
        }

        public bool IsEmpty()
        {
            return value == null;
        }

        public T? Get()
        {
            return value;
        }

        public T? OrElse(T other)
        {
            return IsPresent() ? value : other;
        }

        public T? OrElseGet(Func<T> getOther)
        {
            return IsPresent() ? value : getOther();
        }

        public T? OrElseThrow<E>(Func<E> exceptionSupplier) where E : Exception
        {
            return IsPresent() ? value : throw exceptionSupplier();
        }

        public static explicit operator T(Optional<T> optional)
        {
            return OfNullable((T?)optional).Get();
        }

        public static implicit operator Optional<T>(T optional)
        {
            return OfNullable(optional);
        }
    }
}