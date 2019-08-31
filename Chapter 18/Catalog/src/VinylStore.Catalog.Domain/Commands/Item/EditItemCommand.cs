using System;
using MediatR;
using VinylStore.Catalog.Domain.Entities;
using VinylStore.Catalog.Domain.Responses.Item;

namespace VinylStore.Catalog.Domain.Commands.Item
{
    public class EditItemCommand : IRequest<ItemResponse>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LabelName { get; set; }
        public Money Price { get; set; }
        public string PictureUri { get; set; }
        public DateTimeOffset ReleaseDate { get; set; }
        public string Format { get; set; }
        public int AvailableStock { get; set; }
        public Guid GenreId { get; set; }
        public Guid ArtistId { get; set; }
    }
}
