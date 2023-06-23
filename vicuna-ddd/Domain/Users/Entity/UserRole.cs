using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace vicuna_ddd.Model.Users.Entity
{
    /// <summary>
    /// entity class / dao for user roles
    /// TODO: use for user managment
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
        public virtual UserRoleTypes RoleType { get; set; }
        public virtual string? RoleDescription { get; set; }
    }
}