using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using vicuna_ddd.Domain.Users.Messaging;
using vicuna_ddd.Shared.Entity;

namespace vicuna_ddd.Domain.Messages.Entity
{
    /// <summary>
    ///     entity class for delivered messages
    /// </summary>
    [Table("delivered_message")]
    public class DeliveredMessage : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public long MessageId { get; set; }

        [NotNull] public Guid? Messagekey { get; set; }

        [NotNull] public MessageType MessageType { get; set; }

        [NotNull] public MessageStatus? MessageStatus { get; set; }
    }
}