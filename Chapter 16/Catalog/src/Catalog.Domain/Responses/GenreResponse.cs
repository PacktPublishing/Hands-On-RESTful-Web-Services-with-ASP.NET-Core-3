using System;

namespace Catalog.Domain.Responses
{
    public class GenreResponse
    {
        public Guid GenreId { get; set; }
        public string GenreDescription { get; set; }
    }
}