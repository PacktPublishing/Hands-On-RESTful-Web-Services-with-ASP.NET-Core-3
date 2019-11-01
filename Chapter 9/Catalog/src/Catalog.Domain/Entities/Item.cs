using System;

namespace Catalog.Domain.Entities
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LabelName { get; set; }
        public Price Price { get; set; }
        public string PictureUri { get; set; }
        public DateTimeOffset ReleaseDate { get; set; }
        public string Format { get; set; }
        public int AvailableStock { get; set; }
        public Guid GenreId { get; set; }
        public Genre Genre { get; private set; }
        public Guid ArtistId { get; set; }
        public Artist Artist { get; private set; }
        public bool IsInactive { get; set; }
    }
}