using Dapper.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dapper.Infrastructure.DbContexts
{
    public class DapperDbContext:DbContext
    {
        public DapperDbContext(DbContextOptions<DapperDbContext> options)
            :base(options)
        {}

        public DbSet<Product> Products { get; set; }
    }
}
