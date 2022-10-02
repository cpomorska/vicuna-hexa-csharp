using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Fivevoices.Backend.Db.Context
{
    public static class DbContextExtensions
    {
        public static ChangeTracker ToObjectContext(this DbContext dbContext)
        {
            return dbContext.ChangeTracker;
        }
    }
}