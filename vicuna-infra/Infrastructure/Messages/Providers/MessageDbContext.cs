using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using vicuna_ddd.Domain.Messages.Entity;
using vicuna_ddd.Infrastructure.Shared.Provider;
using vicuna_ddd.Model.Users.Entity;

namespace vicuna_ddd.Shared.Provider
{
    public class MessageDbDbContext : GenericDbContext
    {
        public MessageDbDbContext(bool useInMemoryDb) : base(useInMemoryDb)
        {
            UseInMemoryDb = useInMemoryDb;
        }

        public MessageDbDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<DeliveredMessage>? Users { get; set; }
        public DbSet<UserHash>? UserHash { get; set; }
        public DbSet<UserRole>? UserRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }

            if (!UseInMemoryDb)
            {
                var configuration = DbConfigurationProvider.GetApplicationConfigurationRoot();
                var connectionString = configuration.GetConnectionString("DataAccessPostgreSqlProvider");
                if (connectionString == null || connectionString.StartsWith("none"))
                {
                    optionsBuilder.UseInMemoryDatabase(@"Database=EFProviders.InMemory;Trusted_Connection=True;");
                }
                else
                {
                    optionsBuilder.UseNpgsql(connectionString);
                }
            }
        }
    }
}