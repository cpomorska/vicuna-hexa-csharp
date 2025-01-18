using vicuna_ddd.Domain.Messages.Entity;
using vicuna_ddd.Domain.Users.Messaging;

namespace vicuna_ddd.Domain.Messages.Service
{
    /// Represents a service for retrieving and managing message confirmations.
    /// /
    public interface IReadMessageConformationService
    {
        /// <summary>
        ///     Retrieves a collection of delivered messages of a specific type.
        /// </summary>
        /// <param name="messageType">The type of messages to retrieve confirmations for.</param>
        /// <returns>A collection of delivered messages.</returns>
        Task<IEnumerable<DeliveredMessage?>> GetMessageConfirmationsByType(MessageType messageType);

        /// <summary>
        ///     Retrieves a collection of delivered messages with a specific status.
        /// </summary>
        /// <param name="messageStatus">The status of messages to retrieve confirmations for.</param>
        /// <returns>A collection of delivered messages.</returns>
        Task<IEnumerable<DeliveredMessage?>> GetMesaageConfirmationsByStatus(MessageStatus messageStatus);

        /// <summary>
        ///     Retrieves the confirmation of a delivered message with the specified GUID.
        /// </summary>
        /// <param name="guid">The GUID of the message confirmation to find.</param>
        /// <returns>The delivered message confirmation, or null if not found.</returns>
        Task<DeliveredMessage?> FindMessageConformation(Guid guid);

        /// <summary>
        ///     Retrieves all delivery confirmations.
        /// </summary>
        /// <returns>A collection of delivered messages.</returns>
        Task<IEnumerable<DeliveredMessage>> GetAllDeliveryConfirmations();
    }
}