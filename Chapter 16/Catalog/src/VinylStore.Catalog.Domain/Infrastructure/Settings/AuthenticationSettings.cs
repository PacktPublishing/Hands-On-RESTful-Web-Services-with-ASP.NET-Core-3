namespace VinylStore.Catalog.Domain.Infrastructure.Settings
{
    public class AuthenticationSettings
    {
        public string Secret { get; set; }
        public string ConnectionString { get; set; }
        public int ExpirationDays { get; set; }
    }
}
