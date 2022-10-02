using Fivevoices.Backend.Db.Generic;

namespace vicuna_ddd.Model
{
    public class UserRepository : IGenericRepository<User>
    {
        public void Add(params User[] items)
        {
            throw new NotImplementedException();
        }

        public IList<User> GetAll(params System.Linq.Expressions.Expression<Func<User, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public IList<User> GetList(Func<User, bool> where, params System.Linq.Expressions.Expression<Func<User, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public User GetSingle(Func<User, bool> where, params System.Linq.Expressions.Expression<Func<User, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public void Remove(params User[] items)
        {
            throw new NotImplementedException();
        }

        public void Update(params User[] items)
        {
            throw new NotImplementedException();
        }
    }
}
