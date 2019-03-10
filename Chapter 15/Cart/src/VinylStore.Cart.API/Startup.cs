using System;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            CurrentEnvironment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment CurrentEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services
                .AddScoped<ICartRepository, CartRepository>()
                .AddScoped<ICatalogService, CatalogService>()
                .AddCatalogService(new Uri(Configuration["CatalogApiUrl"]))
                .AddMediatR()
                .AddAutoMapper()
                .AddRabbitMQ(Configuration.GetSection("ESB:EndPointName").Value,
                    Configuration.GetSection("ESB:ConnectionString").Value,
                    CurrentEnvironment.EnvironmentName)
                .Configure<CartDataSourceSettings>(Configuration);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app
                .UseHttpsRedirection()
                .UseMvc();
        }
    }
}
