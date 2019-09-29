using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SampleAPI.Filters
{
    public class CustomActionFilterAsync : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            //Before 
 
            var resultContext = await next();
 
            //After
        }
    }
}