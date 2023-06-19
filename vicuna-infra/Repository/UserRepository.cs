using vicuna_ddd.Infrastructure;
using vicuna_ddd.Model;
using vicuna_ddd.Model.Users.Entity;

namespace vicuna_infra.Repository
{
    public class UserRepository : GenericRepository<UserDbContext, User>
    {
    }
}
