using Newtonsoft.Json;
using vicuna_ddd.Domain.Users.Messaging;

namespace vicuna_ddd.Domain.Users.Dto
{
    /// <summary>
    /// Represents a delivery confirmation message that is sent when a message has been successfully delivered to its destination.
    /// </summary>
    public class DeliveryConfirmationDto
    {
        /// <summary>
        /// Gets or sets the unique key of the corresponding message. This property is required and must be present in JSON data when deserializing an instance of the DTO.
        /// </summary>
        [JsonRequired]
        [JsonProperty("messagekey")]
        public Guid MessageKey { get; set; }

        /// <summary>
        /// Gets or sets the type of message being delivered. This property is required and must be present in JSON data when deserializing an instance of the DTO.
        /// </summary>
        [JsonRequired]
        [JsonProperty("messagetype")]
        public MessageType MessageType { get; set; }

        /// <summary>
        /// Gets or sets the status of the message after delivery. This property is optional and can be omitted in JSON data when deserializing an instance of the DTO.
        /// </summary>
        [JsonProperty("messagestatus")]
        public MessageStatus? MessageStatus { get; set; }
    }
}