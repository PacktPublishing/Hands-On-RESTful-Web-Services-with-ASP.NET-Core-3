using System;
using Microsoft.EntityFrameworkCore;
using VinylStore.Catalog.Infrastructure;

namespace VinylStore.Catalog.Fixtures
{
    public class CatalogDataContextFactory : IDisposable
    {
        public readonly TestCatalogContext ContextInstance;
        public readonly DbContextOptions<CatalogContext> ContextOptions;

        public CatalogDataContextFactory()
        {
            ContextOptions = new DbContextOptionsBuilder<CatalogContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            ContextInstance = new TestCatalogContext(ContextOptions);

            ContextInstance.Database.EnsureCreated();
        }

        public void Dispose()
        {
            ContextInstance.Database.EnsureDeleted();
            ContextInstance.Dispose();
        }
    }
}