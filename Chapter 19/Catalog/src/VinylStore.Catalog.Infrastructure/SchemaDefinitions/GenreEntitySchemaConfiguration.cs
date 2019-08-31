using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VinylStore.Catalog.Domain.Entities;

namespace VinylStore.Catalog.Infrastructure.SchemaDefinitions
{
    public class GenreEntitySchemaConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.ToTable("Genres", CatalogContext.DEFAULT_SCHEMA);
            builder.HasKey(x => x.GenreId);
            builder.Property(x => x.GenreId);
            builder.Property(x => x.GenreDescription)
                .IsRequired()
                .HasMaxLength(1000);
        }
    }
}
