using System;

namespace Catalog.API.Contract.Item
{
    public class ArtistResponse
    {
        public Guid ArtistId { get; set; }
        public string ArtistName { get; set; }
    }
}