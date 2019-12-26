using Microsoft.AspNetCore.Hosting;

namespace Catalog.API.Extensions
{
    public static class WebHostExtensions
    {
        public static bool IsTesting(this IWebHostEnvironment environment) =>
            environment.EnvironmentName == "Testing";
    }
}