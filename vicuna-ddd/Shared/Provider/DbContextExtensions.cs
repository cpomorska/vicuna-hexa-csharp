using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace vicuna_ddd.Shared.Provider
{
    public static class DbContextExtensions
    {
        public static ChangeTracker ToObjectContext(this DbContext dbContext)
        {
            return dbContext.ChangeTracker;
        }
    }
}