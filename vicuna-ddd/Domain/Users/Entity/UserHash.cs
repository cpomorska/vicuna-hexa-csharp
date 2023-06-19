using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace vicuna_ddd.Model.Users.Entity
{
    public class UserHash
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int UserId { get; set; }
        public virtual string? hashField { get; set; }
        public virtual string? saltField { get; set; }
    }
}
