using vicuna_ddd.Domain.Users.Dto;

namespace vicuna_ddd.Domain.Messages.Service
{
    /// <summary>
    ///     Represents a service for writing and managing message delivery confirmations.
    /// </summary>
    public interface IWriteMessageConfirmationService
    {
        /// <summary>
        ///     Stores the delivered message confirmation in the system.
        /// </summary>
        /// <param name="messageDelivery">The delivery confirmation message object.</param>
        /// <returns>
        ///     A task representing the asynchronous operation. The task result contains the identifier of the stored message
        ///     confirmation if successful,
        ///     or null if there was an error storing the message confirmation.
        /// </returns>
        Task<Guid?> StoreDeliveredMessage(DeliveryConfirmationDto messageDelivery);

        /// <summary>
        ///     Updates the delivered message with the specified id.
        /// </summary>
        /// <param name="id">The id of the delivered message.</param>
        /// <returns>The id of the updated delivered message, or null if the update failed.</returns>
        Task<Guid?> UpdateDeliveredMessage(Guid? id);

        /// <summary>
        ///     Deletes a delivered message.
        /// </summary>
        /// <param name="id">The ID of the message to delete.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        Task<Guid?> DeleteDeliveredMessage(Guid id);
    }
}