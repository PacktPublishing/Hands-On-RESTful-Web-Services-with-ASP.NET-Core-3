using System.Reflection;
using Catalog.Domain.Mappers;
using Catalog.Domain.Services;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Domain.Extensions
{
    public static class DependenciesRegistration
    {
        public static IServiceCollection AddMappers(this IServiceCollection services)
        {
            services
                .AddSingleton<IArtistMapper, ArtistMapper>()
                .AddSingleton<IGenreMapper, GenreMapper>()
                .AddSingleton<IItemMapper, ItemMapper>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddScoped<IItemService, ItemService>()
                .AddScoped<IArtistService, ArtistService>()
                .AddScoped<IGenreService, GenreService>();
            return services;
        }

        public static IMvcBuilder AddValidation(this IMvcBuilder builder)
        {
            builder
                .AddFluentValidation(configuration =>
                    configuration.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

            return builder;
        }
    }
}