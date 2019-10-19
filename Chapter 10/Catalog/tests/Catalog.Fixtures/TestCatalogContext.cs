using Catalog.Domain.Entities;
using Catalog.Fixtures.Extensions;
using Catalog.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Fixtures
{
    public class TestCatalogContext : CatalogContext
    {
        public TestCatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed<Artist>("./Data/artist.json");
            modelBuilder.Seed<Genre>("./Data/genre.json");
            modelBuilder.Seed<Item>("./Data/item.json");

            base.OnModelCreating(modelBuilder);
        }
    }
}