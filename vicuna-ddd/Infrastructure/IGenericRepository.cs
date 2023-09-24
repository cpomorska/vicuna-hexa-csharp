using System.Linq.Expressions;
using vicuna_ddd.Shared.Entity;

namespace vicuna_ddd.Infrastructure
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IList<T>> GetAll(params Expression<Func<T, object>>[] navigationProperties);
        Task<IList<T>> GetList(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties);
        Task<T> GetSingle(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties);
        Task Add(params T[] items);
        Task Update(params T[] items);
        Task Remove(params T[] items);
    }
}