using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace vicuna_ddd.Model.Users.Entity
{
    /// <summary>
    /// entity class / dao for user roles
    /// used for user management
    /// </summary> 
    [Table("vicunauserrole")]
    public class UserRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int RoleId { get; set; }
        [NotNull]
        public virtual string RoleName { get; set; }
        [NotNull]
        [EnumDataType(typeof(UserRoleTypes))]
        public UserRoleTypes RoleType { get; set; }
        public string? RoleDescription { get; set; }
    }
}