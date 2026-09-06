using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace vicuna_ddd.Infrastructure.Shared.Entity
{
    public class BaseEntity
    {
        [Required] public DateTime? CreatedAt { get; init; } = DateTime.Now;

        [JsonIgnore] public DateTime? ModifiedAt { get; set; }

        [JsonIgnore] public string? ModifiedFrom { get; set; }
    }
}