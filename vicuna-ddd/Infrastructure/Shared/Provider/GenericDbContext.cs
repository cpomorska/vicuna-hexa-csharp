using Microsoft.EntityFrameworkCore;

namespace vicuna_ddd.Shared.Provider
{
    public class GenericDbContext : DbContext
    {
        public GenericDbContext()
        {
        }

        public GenericDbContext(bool useInMemoryDb)
        {
            UseInMemoryDb = useInMemoryDb;
        }

        public GenericDbContext(DbContextOptions options) : base(options)
        {
        }

        public bool UseInMemoryDb { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
            }
            else
            {
                optionsBuilder.UseInMemoryDatabase(@"Database=EFProviders.InMemory;Trusted_Connection=True;");
            }
        }
    }
}