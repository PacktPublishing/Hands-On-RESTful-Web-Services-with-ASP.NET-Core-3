namespace VinylStore.Catalog.API.Infrastructure.Filters
{
    public class JsonErrorPayload
    {
        public int EventId { get; set; }
        public object DetailedMessage { get; set; }
    }
}