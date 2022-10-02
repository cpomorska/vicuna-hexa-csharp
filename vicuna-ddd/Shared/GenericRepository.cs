using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using vicuna_ddd.Shared;

namespace Fivevoices.Backend.Db.Generic
{
    public class GenericRepository<TDbContext, T> : IGenericRepository<T>
        where TDbContext : GenericDbContext
    where T : class
    {
        public bool UnitTestDb { get; set; }

        public void Add(params T[] items)
        {
            using (var context = new GenericDbContext(UnitTestDb))
            {
                foreach (var item in items)
                {
                    context.Set<T>().Add(item);
                    context.SaveChanges();
                }
            }
        }

        public IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list;
            using (var context = new GenericDbContext(UnitTestDb))
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .ToList();
            }
            return list;
        }

        public IList<T> GetList(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list;
            using (var context = new GenericDbContext(UnitTestDb))
            {
                IQueryable<T> dbQuery = context.Set<T>();

                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .Where(where)
                    .ToList();
            }
            return list;
        }

        public T GetSingle(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties)
        {
            T item;
            using (var context = new GenericDbContext(UnitTestDb))
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include(navigationProperty);

                item = dbQuery
                    .AsNoTracking()
                    .FirstOrDefault(where);
            }
            return item;
        }

        public void Remove(params T[] items)
        {
            using (var context = new GenericDbContext(UnitTestDb))
            {
                foreach (var item in items)
                {
                    context.Set<T>().Remove(item);
                    context.SaveChanges();
                }
                
            }
        }

        public void Update(params T[] items)
        {
            using (var context = new GenericDbContext(UnitTestDb))
            {
                foreach (var item in items)
                {
                    context.Set<T>().Update(item);
                    context.SaveChanges();
                }
            }
        }
    }
}