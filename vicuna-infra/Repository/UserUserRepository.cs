using vicuna_ddd.Infrastructure;
using vicuna_ddd.Infrastructure.Users.Repository;
using vicuna_ddd.Model.Users.Entity;
using vicuna_ddd.Shared.Provider;

namespace vicuna_infra.Repository
{
    public class UserUserRepository : GenericUserRepository<UserDbContext, User>
    {
    }
}