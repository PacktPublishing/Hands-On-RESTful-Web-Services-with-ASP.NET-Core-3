using System;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VinylStore.Cart.Domain.Infrastructure.Repositories;
using VinylStore.Cart.Domain.Services;
using VinylStore.Cart.Infrastructure.Configurations;
using VinylStore.Cart.Infrastructure.Extensions;
using VinylStore.Cart.Infrastructure.Repositories;
using VinylStore.Cart.Infrastructure.Services;

namespace VinylStore.Cart.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            CurrentEnvironment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment CurrentEnvironment { get; }

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
                .AddRabbitMQ(Configuration.GetSection("ESB:EndPointName").Value,
                    Configuration.GetSection("ESB:ConnectionString").Value,
                    CurrentEnvironment.EnvironmentName)
                .Configure<CartDataSourceSettings>(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app
                .UseRouting()
                .UseHttpsRedirection()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
        }
    }
}
