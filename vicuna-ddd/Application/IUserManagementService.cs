using vicuna_ddd.Model.Users.Entity;

namespace vicuna_infra.Service
{
    public interface IUserManagementService
    {
        Task<Guid?> AddUser(User user);
        Task<Guid?> RemoveUser(User user);
        Task<Guid?> UpdateUser(User user);
    }
}