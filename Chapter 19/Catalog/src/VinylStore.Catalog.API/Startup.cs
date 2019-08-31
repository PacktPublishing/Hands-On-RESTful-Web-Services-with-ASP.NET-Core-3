using System;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RiskFirst.Hateoas;
using VinylStore.Catalog.API.Controllers;
using VinylStore.Catalog.API.Infrastructure.Extensions;
using VinylStore.Catalog.API.Infrastructure.HealthCheck;
using VinylStore.Catalog.API.Infrastructure.Middleware;
using VinylStore.Catalog.API.ResponseModels;
using VinylStore.Catalog.Domain.Infrastructure.Extensions;
using VinylStore.Catalog.Domain.Infrastructure.Repositories;
using VinylStore.Catalog.Domain.Infrastructure.Settings;
using VinylStore.Catalog.Infrastructure;
using VinylStore.Catalog.Infrastructure.Repositories;
using NSwag;

namespace VinylStore.Catalog.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var dataSourceConnectionString = Configuration.GetSection("DataSource:ConnectionString");
            var cachingConnectionString = Configuration.GetSection("Caching");

            services.Configure<CacheSettings>(cachingConnectionString);

            services
                .AddCatalogContext(dataSourceConnectionString.Value)
                .AddCustomAuthentication(Configuration)
                .AddScoped<IItemRepository, ItemRepository>()
                .AddScoped<IArtistRepository, ArtistRepository>()
                .AddScoped<IGenreRepository, GenreRepository>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddResponseCaching()
                .AddDomainComponents()
                .AddLogging(_ => _.AddConsole())
                .AddDistributedRedisCache(options =>
                {
                    options.Configuration = cachingConnectionString.Get<CacheSettings>().ConnectionString;
                })
                .AddControllers()
                .AddFluentValidation();
                
            services
                .AddHealthChecks()
                .AddSqlServer(dataSourceConnectionString.Value)
                .AddCheck<RedisCacheHealthCheck>("cache_health_check");
               
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
            
            services.AddOpenApiDocument(settings =>{
                settings.Title = "Vinyl catalog service";
                settings.DocumentName = "v3";
                settings.Version = "v3";                                   
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            try
            {
                app.ApplicationServices.GetService<CatalogContext>().Database.Migrate();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            app
                .UseRouting()
                .UseHttpsRedirection()
                .UseMiddleware<ResponseTimeMiddlewareAsync>()
                .UseResponseCaching()
                .UseHealthChecks("/health")
                .UseAuthentication()
                .UseAuthorization()
                .UseOpenApi()
                .UseSwaggerUi3()
                .UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
