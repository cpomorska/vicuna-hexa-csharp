using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace vicuna_ddd.Shared.Entity
{
    public class BaseEntity
    {
        [JsonIgnore]
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public DateTime? ModifiedAt { get; set; }
        [JsonIgnore]
        public string? ModifiedFrom { get; set; }
    }
}
