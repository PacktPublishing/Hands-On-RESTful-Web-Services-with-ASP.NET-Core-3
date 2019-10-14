using System;
using AutoMapper;
using Catalog.Domain.Requests.Item;
using Catalog.Domain.Requests.Item.Validators;
using Catalog.Domain.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Domain.Extensions
{
    public static class DomainExtensions
    {
        public static IServiceCollection AddDomainComponents(this IServiceCollection services)
        {
            services
                .AddScoped<IItemService, ItemService>()
                .AddScoped<IGenreService, GenreService>()
                .AddScoped<IArtistService, ArtistService>()
                .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
                .AddTransient<IValidator<EditItemRequest>, EditItemRequestValidator>()
                .AddTransient<IValidator<AddItemRequest>, AddItemRequestValidator>();

            return services;
        }
    }
}