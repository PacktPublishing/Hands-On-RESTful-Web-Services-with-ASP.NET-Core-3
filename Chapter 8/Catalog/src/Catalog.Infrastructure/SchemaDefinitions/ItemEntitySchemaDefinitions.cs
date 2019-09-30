using System;
using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.SchemaDefinitions
{
    public class ItemEntitySchemaDefinition : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("Items", CatalogContext.DEFAULT_SCHEMA);

            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.Name)
                .IsRequired();

            builder.Property(_ => _.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder
                .HasOne(e => e.Genre)
                .WithMany(c => c.Items)
                .HasForeignKey(_ => _.GenreId);

            builder
                .HasOne(e => e.Artist)
                .WithMany(c => c.Items)
                .HasForeignKey(_ => _.ArtistId);

            builder.Property(_ => _.Price).HasConversion(
                _ => $"{_.Amount}:{_.Currency}",
                _ => new Money
                {
                    Amount = Convert.ToDecimal(_.Split(":", StringSplitOptions.None)[0]),
                    Currency = _.Split(":", StringSplitOptions.None)[1]
                });
        }
    }
}