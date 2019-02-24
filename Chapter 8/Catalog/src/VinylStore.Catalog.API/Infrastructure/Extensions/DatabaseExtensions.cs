using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VinylStore.Catalog.Infrastructure;

namespace VinylStore.Catalog.API.Infrastructure.Extensions
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