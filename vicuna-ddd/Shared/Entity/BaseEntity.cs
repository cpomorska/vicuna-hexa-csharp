using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace vicuna_ddd.Shared.Entity
{
    public class BaseEntity
    {
        [Required] 
        public DateTime? CreatedAt { get; init; } = DateTime.Now;
        [JsonIgnore]
        public DateTime? ModifiedAt { get; set; }
        [JsonIgnore]
        public string? ModifiedFrom { get; set; }
    }
}