using Microsoft.AspNetCore.Identity;

namespace VinylStore.Catalog.Domain.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
    }
}
