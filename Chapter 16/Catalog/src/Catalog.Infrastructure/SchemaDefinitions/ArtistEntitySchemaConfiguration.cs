using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.SchemaDefinitions
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