using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using vicuna_ddd.Shared.Entity;
using System.Text.Json.Serialization;

namespace vicuna_ddd.Model.Users.Entity
{
    [Table("vicunauserhash")]
    public class UserHash
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public long UserHashId { get; set; }
        public string? hashField { get; set; }
        public string? saltField { get; set; }
    }
}
