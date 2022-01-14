using Dapper.Core.Entities;
using Dapper.Core.Interfaces;
using Dapper.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Dapper.Infrastructure
{
    public static class ServiceRegisteration
    {
        public static void AddInfrastructure (this IServiceCollection services)
        {
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IDb<Product>, Db<Product>>();
        }
    }
}
