using System.Reflection;
using Catalog.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.API.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddCatalogContext(this IServiceCollection services, string connectionString)
        {
            return services
                .AddEntityFrameworkSqlServer()
                .AddDbContext<CatalogContext>(opt =>
                {
                    opt.UseSqlServer(
                        connectionString,
                        _ =>
                        {
                            _.MigrationsAssembly(typeof(Startup)
                                .GetTypeInfo()
                                .Assembly
                                .GetName().Name);
                        });
                });
        }
    }
}