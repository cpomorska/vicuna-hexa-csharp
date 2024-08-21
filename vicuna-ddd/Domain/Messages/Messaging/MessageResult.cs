namespace vicuna_ddd.Domain.Users.Messaging
{
    /// <summary>
    /// Represents the result of an operation.
    /// </summary>
    public enum MessageResult
    {
        /// <summary>
        /// The operation was successful.
        /// </summary>
        Ok,

        /// <summary>
        /// The operation failed.
        /// </summary>
        NoK,

        /// <summary>
        /// The result of the operation is unknown
        /// </summary>
        Unknown
    }
}
