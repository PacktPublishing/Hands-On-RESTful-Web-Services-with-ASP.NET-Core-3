using Microsoft.AspNetCore.Identity;

namespace Catalog.Domain.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
    }
}