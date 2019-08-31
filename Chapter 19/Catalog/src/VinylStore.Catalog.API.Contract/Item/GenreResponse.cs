using System;

namespace VinylStore.Catalog.API.Contract.Item
{
    public class GenreResponse
    {
        public Guid GenreId { get; set; }
        public string GenreDescription { get; set; }
    }
}
