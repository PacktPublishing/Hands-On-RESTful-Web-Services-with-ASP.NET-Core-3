using System;
using Catalog.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Fixtures
{
    public class InMemoryApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseEnvironment("Testing")
                .UseSolutionRelativeContentRoot("")
                .ConfigureTestServices(services =>
                {
                    var options = new DbContextOptionsBuilder<CatalogContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString())
                        .Options;

                    services.AddScoped<CatalogContext>(serviceProvider => new TestCatalogContext(options));
                    var sp = services.BuildServiceProvider();

                    using var scope = sp.CreateScope();
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<CatalogContext>();
                    db.Database.EnsureCreated();
                });

        }
    }
}