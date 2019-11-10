using Catalog.API.Controllers;
using Catalog.API.Extensions;
using Catalog.API.HealthChecks;
using Catalog.API.Middleware;
using Catalog.API.ResponseModels;
using Catalog.Domain.Extensions;
using Catalog.Domain.Repositories;
using Catalog.Infrastructure;
using Catalog.Infrastructure.Extensions;
using Catalog.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RiskFirst.Hateoas;

namespace Catalog.API
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private IWebHostEnvironment CurrentEnvironment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment currentEnvironment)
        {
            Configuration = configuration;
            CurrentEnvironment = currentEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCatalogContext(Configuration.GetSection("DataSource:ConnectionString").Value)
                .AddScoped<IItemRepository, ItemRepository>()
                .AddScoped<IArtistRepository, ArtistRepository>()
                .AddScoped<IGenreRepository, GenreRepository>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddTokenAuthentication(Configuration)
                .AddMappers()
                .AddServices()
                .AddResponseCaching()
                .AddOpenApiDocument(settings =>{
                    settings.Title = "Catalog API";
                    settings.DocumentName = "v3";
                    settings.Version = "v3";                                   
                })
                .AddDistributedRedisCache(Configuration)
                .AddControllers()
                .AddValidation();

            services
                .AddHealthChecks()
                .AddCheck<RedisCacheHealthCheck>("cache_health_check")
                .AddSqlServer(Configuration.GetSection("DataSource:ConnectionString").Value);

            services.AddRabbitMq(
                    Configuration.GetSection("ESB:EndPointName").Value,
                Configuration.GetSection("ESB:ConnectionString").Value,
                    CurrentEnvironment.EnvironmentName)
            .GetAwaiter()
            .GetResult();

            services.AddLinks(config =>
            {
                config.AddPolicy<ItemHateoasResponse>(policy =>
                {
                    policy
                        .RequireRoutedLink(nameof(ItemsHateoasController.Get), nameof(ItemsHateoasController.Get))
                        .RequireRoutedLink(nameof(ItemsHateoasController.GetById),
                            nameof(ItemsHateoasController.GetById), _ => new { id = _.Data.Id })
                        .RequireRoutedLink(nameof(ItemsHateoasController.Post), nameof(ItemsHateoasController.Post))
                        .RequireRoutedLink(nameof(ItemsHateoasController.Put), nameof(ItemsHateoasController.Put),
                            x => new { id = x.Data.Id })
                        .RequireRoutedLink(nameof(ItemsHateoasController.Delete), nameof(ItemsHateoasController.Delete),
                            x => new { id = x.Data.Id });
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsTesting())
                app.ApplicationServices.GetService<CatalogContext>().Database.Migrate();


            app.UseResponseCaching();
            app.UseHealthChecks("/health");
            app.UseRouting();
            app.UseOpenApi();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwaggerUi3();
            app.UseHttpsRedirection();
            app.UseMiddleware<ResponseTimeMiddlewareAsync>();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}