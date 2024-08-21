using vicuna_ddd.Domain.Users.Dto;
using vicuna_ddd.Model.Users.Entity;

namespace vicuna_infra.Service
{
    public interface IUserService
    {
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserByUsername(string username);
        Task<User?> GetUserByUsernnameAndPassword(string userName, string password);
        Task<User?> FindUser(UserDto userDto);
    }
}