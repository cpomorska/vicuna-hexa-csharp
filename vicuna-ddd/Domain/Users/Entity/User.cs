using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace vicuna_ddd.Model.Users.Entity
{
    /// <summary>
    /// entity class / dao for users
    /// </summary>
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [NotNull]
        public string? UserNumber { get; set; }
        [NotNull]
        public string? UserToken { get; set; }
        [NotNull]
        public string? UserName { get; set; }
        [NotNull]
        public string? UserPass { get; set; }
        [NotNull]
        public string? UserEmail { get; set; }
        [NotNull]
        public bool UserEnabled { get; set; }
        [NotNull]
        public virtual UserRole? UserRole { get; set; }
        [NotNull]
        public virtual UserHash? UserHash { get; set; }
    }
}
