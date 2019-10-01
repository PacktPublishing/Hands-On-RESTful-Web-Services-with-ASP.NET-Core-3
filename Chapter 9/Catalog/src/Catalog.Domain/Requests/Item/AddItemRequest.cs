using System;
using Catalog.Domain.Entities;
using Catalog.Domain.Responses.Item;
using MediatR;

namespace Catalog.Domain.Requests.Item
{
    public class AddItemRequest : IRequest<ItemResponse>
    {
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