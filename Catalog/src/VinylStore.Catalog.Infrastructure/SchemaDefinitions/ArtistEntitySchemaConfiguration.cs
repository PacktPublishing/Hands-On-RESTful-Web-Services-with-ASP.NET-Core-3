using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VinylStore.Catalog.Domain.Entities;

namespace VinylStore.Catalog.Infrastructure.SchemaDefinitions
{
    public class ArtistEntitySchemaConfiguration : IEntityTypeConfiguration<Artist>
    {
        public void Configure(EntityTypeBuilder<Artist> builder)
        {
            builder.ToTable("Artists", CatalogContext.DEFAULT_SCHEMA);
            builder.HasKey(x => x.ArtistId);
            builder.Property(x => x.ArtistId);

            builder.Property(x => x.ArtistName)
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}