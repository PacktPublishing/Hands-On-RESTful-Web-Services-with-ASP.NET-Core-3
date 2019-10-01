using System;
using Catalog.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Fixtures
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

            EnsureCreation(ContextOptions);
            ContextInstance = new TestCatalogContext(ContextOptions);
        }

        private void EnsureCreation(DbContextOptions<CatalogContext> contextOptions)
        {
            using var context = new TestCatalogContext(contextOptions);
            context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            ContextInstance.Database.EnsureDeleted();
            ContextInstance.Dispose();
        }
    }
}