using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using vicuna_ddd.Shared;

namespace vicuna_ddd.Model
{
    public class UserDbContext : GenericDbContext
    {
        public DbSet<User>? Users { get; set; }
        public DbSet<UserHash>? UserHash { get; set; }
        public DbSet<UserRole>? UserRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;

            if (!UseInMemoryDb)
            {
                var configuration = DbConfigurationProvider.GetApplicationConfigurationRoot();
                var connectionString = configuration.GetConnectionString("DataAccessPostgreSqlProvider");
                optionsBuilder.UseNpgsql(connectionString);
            }
            else
            {
                optionsBuilder.UseInMemoryDatabase(@"Database=EFProviders.InMemory;Trusted_Connection=True;");
            }
        }
    }
}
