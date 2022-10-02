using Microsoft.EntityFrameworkCore;

namespace vicuna_ddd.Shared
{
    public class GenericDbContext : DbContext
    {
        public bool UseInMemoryDb { get; set; }

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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;
            else
            {
                optionsBuilder.UseInMemoryDatabase(@"Database=EFProviders.InMemory;Trusted_Connection=True;");
            }
        }
    }
}
