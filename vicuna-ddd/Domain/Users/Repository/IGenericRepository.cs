using System.Linq.Expressions;
using vicuna_ddd.Shared.Entity;

namespace vicuna_ddd.Domain.Users.Repository
{
    /// <summary>
    /// Generic Repository Interface, which is used for the GenericRepository 
    /// for the Domains Repository
    /// </summary>
    /// <typeparam name="T">Generic Type "T" which should extend baseEntity</typeparam>
    public interface IGenericUserRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Ansync Task which gets all entities from Repo, used for tests
        /// </summary>
        /// <param name="navigationProperties">Fucntion which takes lambdas</param>
        /// <returns>If the Tssk is finished a List of "T", BaseEntities, is returned</returns>
        Task<IList<T>> GetAll(params Expression<Func<T, object>>[] navigationProperties);

        /// <summary>
        /// Ansync Task which gets all entities from Repo
        /// </summary>
        /// <param name="where">Where Clause</param>
        /// <param name="navigationProperties">Function which takes lambdas</param>
        /// <returns>If the Task is finished a List of "T", BaseEntities, is returned</returns>
        Task<IList<T>> GetList(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties);

        /// <summary>
        /// Ansync Task which gets a single Entity from Repo
        /// </summary>
        /// <param name="where">Where Clause</param>
        /// <param name="navigationProperties">Function which takes lambdas</param>
        /// <returns>If the Task is finished an object of "T", BaseEntity, is returned</returns>
        Task<T> GetSingle(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties);

        /// <summary>
        /// Adds an array of entities to the Database
        /// </summary>
        /// <param name="items">an array of "T" items</param>
        /// <returns>a Task</returns>
        Task Add(params T[] items);

        /// <summary>
        /// Updates an array of entities in the Database
        /// </summary>
        /// <param name="items">an array of "T" items</param>
        /// <returns>a Task</returns>
        Task Update(params T[] items);

        /// <summary>
        /// Removes an array of entities from the Database
        /// </summary>
        /// <param name="items">an array of "T" items</param>
        /// <returns>a Task</returns>
        Task Remove(params T[] items);
    }
}