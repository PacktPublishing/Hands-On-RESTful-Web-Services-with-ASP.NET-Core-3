using System;
//using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RiskFirst.Hateoas;
using VinylStore.Catalog.API.Controllers;
using VinylStore.Catalog.API.Infrastructure.Extensions;
using VinylStore.Catalog.API.Infrastructure.Middleware;
using VinylStore.Catalog.API.ResponseModels;
using VinylStore.Catalog.Domain.Commands.Genre;
using VinylStore.Catalog.Domain.Infrastructure.Extensions;
using VinylStore.Catalog.Domain.Infrastructure.Repositories;
using VinylStore.Catalog.Infrastructure;
using VinylStore.Catalog.Infrastructure.Repositories;

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
            services
                .AddCatalogContext(Configuration.GetSection("DataSource:ConnectionString").Value)
                .AddScoped<IItemRepository, ItemRepository>()
                .AddScoped<IArtistRepository, ArtistRepository>()
                .AddScoped<IGenreRepository, GenreRepository>()
                .AddMediatorComponents()
                .AddMvc()
                .AddNewtonsoftJson();
            //   .AddFluentValidation();

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

        public void Configure(IApplicationBuilder app)
        {
            try
            {
                app.ApplicationServices.GetService<CatalogContext>().Database.Migrate();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<ResponseTimeMiddlewareAsync>();
            app.UseMvc();
        }
    }
}