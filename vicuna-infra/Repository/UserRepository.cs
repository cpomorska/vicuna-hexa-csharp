﻿using vicuna_ddd.Infrastructure;
using vicuna_ddd.Model.Users.Entity;
using vicuna_ddd.Shared.Provider;

namespace vicuna_infra.Repository
{
    public class UserRepository : GenericRepository<UserDbContext, User>
    {
    }
}
