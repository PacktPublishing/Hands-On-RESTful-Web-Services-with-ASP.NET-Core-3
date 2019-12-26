using System.Reflection;
using System.Text;
using Catalog.Domain.Entities;
using Catalog.Domain.Mappers;
using Catalog.Domain.Repositories;
using Catalog.Domain.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

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
                .AddScoped<IGenreService, GenreService>()
                .AddScoped<IUserService, UserService>();

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