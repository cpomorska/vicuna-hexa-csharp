using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace vicuna_ddd.Model
{
    /// <summary>
    /// entity class / dao for user roles
    /// TODO: use for user managment
    /// </summary>
    public class UserRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Int32 RoleId { get; set; }
        [NotNull]
        public virtual String RoleName { get; set; }
        [NotNull]
        [EnumDataType(typeof(UserRoleTypes))]
        public virtual UserRoleTypes RoleType { get; set; }
        public virtual String? RoleDescription { get; set; }
    }
}