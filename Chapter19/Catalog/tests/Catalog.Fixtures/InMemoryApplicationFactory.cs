using System;
using Catalog.Fixtures.Logging;
using Catalog.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Catalog.Fixtures
{
    public class InMemoryApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        private ITestOutputHelper _testOutputHelper;

        public void SetTestOutputHelper(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseEnvironment("Testing")
                .ConfigureTestServices(services =>
                {
                    var options = new DbContextOptionsBuilder<CatalogContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString())
                        .Options;

                    if (_testOutputHelper != null)
                    {
                        services.AddLogging(cfg => cfg.AddProvider(new TestOutputLoggerProvider(_testOutputHelper)));
                    }

                    services.AddScoped<CatalogContext>(serviceProvider => new TestCatalogContext(options));
                    services.Replace(ServiceDescriptor.Scoped(_ => new UsersContextFactory().InMemoryUserManager));
                    services.AddSingleton<IDistributedCache, MemoryDistributedCache>();

                    var sp = services.BuildServiceProvider();

                    using var scope = sp.CreateScope();
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<CatalogContext>();
                    db.Database.EnsureCreated();
                });
        }
    }
}