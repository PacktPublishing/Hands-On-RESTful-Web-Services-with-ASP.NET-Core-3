namespace VinylStore.Catalog.Domain
{
    public class AuthenticationSettings
    {
        public string Secret { get; set; }
        public string ConnectionString { get; set; }
        public int ExpirationDays { get; set; }
    }
}
