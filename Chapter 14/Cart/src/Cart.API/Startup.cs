using System;
using AutoMapper;
using Cart.Domain.Repositories;
using Cart.Domain.Services;
using Cart.Infrastructure.BackgroundServices;
using Cart.Infrastructure.Configurations;
using Cart.Infrastructure.Extensions;
using Cart.Infrastructure.Repositories;
using Cart.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cart.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment CurrentEnvironment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            CurrentEnvironment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers();

            services
                .AddScoped<ICartRepository, CartRepository>()
                .AddScoped<ICatalogService, CatalogService>()
                .AddCatalogService(new Uri(Configuration["CatalogApiUrl"]))
                .AddMediatR(AppDomain.CurrentDomain.GetAssemblies())
                .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
                .AddEventBus(Configuration)
                .AddHostedService<ItemSoldOutBackgroundService>()
                .Configure<CartDataSourceSettings>(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app
                .UseRouting()
                .UseHttpsRedirection()
                .UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}