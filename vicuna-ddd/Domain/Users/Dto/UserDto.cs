using System.Diagnostics.CodeAnalysis;
using vicuna_ddd.Model.Users.Entity;

namespace vicuna_ddd.Domain.Users.Dto
{
    public class UserDto
    {
        public Guid UserNumber { get; set; }
        public string? UserName { get; set; }
        public string? UserPass { get; set; }
        public string? UserEmail { get; set; }
        public bool UserEnabled { get; set; }
        public UserRole? UserRole { get; set; }
    }
}