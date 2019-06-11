using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VinylStore.Catalog.Domain.Entities;

namespace VinylStore.Catalog.Infrastructure.SchemaDefinitions
{
    public class ItemEntitySchemaDefinition : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("Items", CatalogContext.DEFAULT_SCHEMA);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired();

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder
                .HasOne(e => e.Genre)
                .WithMany(c => c.Items)
                .HasForeignKey(x => x.GenreId);

            builder
                .HasOne(e => e.Artist)
                .WithMany(c => c.Items)
                .HasForeignKey(x => x.ArtistId);

            builder.Property(x => x.Price).HasConversion(
                x => $"{x.Amount}:{x.Currency}",
                x => new Money
                {
                    Amount = Convert.ToDecimal(x.Split(':', StringSplitOptions.None)[0]),
                    Currency = x.Split(':', StringSplitOptions.None)[1]
                });
        }
    }
}