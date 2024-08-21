using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vicuna_ddd.Shared.Provider
{
    public class DbInitializer
    {
        private readonly UserDbContext _context;

        public DbInitializer(UserDbContext context)
        {
            _context = context;
        }

        public void Run()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }
    }
}
