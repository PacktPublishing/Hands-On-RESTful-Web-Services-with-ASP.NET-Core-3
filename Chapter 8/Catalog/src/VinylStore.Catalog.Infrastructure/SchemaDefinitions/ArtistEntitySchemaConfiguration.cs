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
            builder.HasKey(_ => _.ArtistId);
            builder.Property(_ => _.ArtistId);

            builder.Property(_ => _.ArtistName)
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}