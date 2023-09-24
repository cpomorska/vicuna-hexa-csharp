﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using vicuna_ddd.Shared.Entity;
using vicuna_ddd.Shared.Provider;

namespace vicuna_ddd.Infrastructure
{
    public class GenericRepository<TDbContext, T> : IGenericRepository<T> where T :BaseEntity
        where TDbContext : GenericDbContext
    {

        public bool UnitTestDb { get; set; }

        public GenericRepository() 
        {
            using (var context = new UserDbContext(UnitTestDb))
            {
                _ = context.Database.EnsureCreatedAsync();
            }
        }


        public async Task Add(params T[] items)
        {
            using (var context = new UserDbContext(UnitTestDb))
            {
                await context.Set<T>().AddRangeAsync(items);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IList<T>> GetAll(params Expression<Func<T, object>>[] navigationProperties)
        {
            using (var context = new UserDbContext(UnitTestDb))
            {
                IQueryable<T> dbQuery = context.Set<T>();

                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include(navigationProperty);

                return await dbQuery
                    .AsNoTracking()
                    .ToListAsync();
            }
        }

        public async Task<IList<T>> GetList(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties)
        {
            using (var context = new UserDbContext(UnitTestDb))
            {
                IQueryable<T> dbQuery = context.Set<T>();

                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include(navigationProperty);

                return await dbQuery
                    .AsNoTracking()
                    .Where(where)
                    .ToListAsync();
            }
        }

        public async Task<T?> GetSingle(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties)
        {
            using (var context = new UserDbContext(UnitTestDb))
            {
                IQueryable<T> dbQuery = context.Set<T>();

                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include(navigationProperty);

                return await dbQuery
                    .AsNoTracking()
                    .FirstOrDefaultAsync(where);
            }
        }

        public async Task Remove(params T[] items)
        {
            using (var context = new UserDbContext(UnitTestDb))
            {
                context.Set<T>().RemoveRange(items);                        
                await context.SaveChangesAsync();
            }
        }

        public async Task Update(params T[] items)
        {
            using (var context = new UserDbContext(UnitTestDb))
            {
                context.Set<T>().UpdateRange(items);
                await context.SaveChangesAsync();
            }
        }
    }
}