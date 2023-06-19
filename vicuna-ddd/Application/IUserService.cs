using vicuna_ddd.Domain.Users.Dto;
using vicuna_ddd.Model.Users.Entity;

namespace vicuna_infra.Service
{
    public interface IUserService
    {
        User? GetUserByEmail(string email);
        User? GetUserByUsername(string username);
        User? GetUserByUsernnameAndPassword(string userName, string password);
        User? FindUser(UserDto userDto);
    }
}